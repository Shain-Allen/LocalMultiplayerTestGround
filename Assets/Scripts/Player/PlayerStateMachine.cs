using System;
using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerStateMachine : StateMachine
    {
        // Player Component References
        [Header("Player Component References")]
        public CharacterController _characterController;
        private Animator _animator;
        private InputActionAsset _inputActionsAsset;
        private InputActionMap _playerInputActionMap;
        private Camera _playerCamera;
        
        // Movement Variables
        [Header("Player Movement Variables")] [SerializeField, Min(0)]
        private float _moveSpeed = 5;

        [SerializeField, Min(0)] private float _maxJumpHeight = 1.0f;
        [SerializeField, Min(0)] private float _maxJumpTime = 0.5f;
        [SerializeField, Min(0)] private float _fallSpeedMultiplier = 2f;
        [SerializeField] private float _maxFallSpeed = 20f;
        private Vector2 MovementInput { get; set; }
        public bool IsMovementPressed { get; private set; }
        [HideInInspector] public Vector3 _currentMovement;
        [HideInInspector] public Vector3 _appliedMovement;

        // gravity variables
        private float _gravity = -9.8f;
        [HideInInspector] public float _groundedGravity = -0.5f;

        // Jumping Variables
        public bool IsJumpPressed { get; private set; }
        public float InitialJumpVelocity { get; private set; }
        [HideInInspector] public bool IsJumping = false;

        private void Awake()
        {
            // Connect player component references
            _states = new StateFactory(this);
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _inputActionsAsset = GetComponent<PlayerInput>().actions;
            _playerInputActionMap = _inputActionsAsset.FindActionMap("Player");
            _playerCamera = transform.parent.GetComponentInChildren<Camera>();

            //Setup jump variables
            SetupJumpVariable();
        }
        
        // subscribe to the players input actions
        private void OnEnable()
        {
            _playerInputActionMap.FindAction("Move").performed += OnMove;
            _playerInputActionMap.FindAction("Move").canceled += OnMove;
            _playerInputActionMap.FindAction("Jump").performed += OnJump;
            _playerInputActionMap.FindAction("Jump").canceled += OnJump;
        }
        
        private void OnDisable()
        {
            _playerInputActionMap.FindAction("Move").performed -= OnMove;
            _playerInputActionMap.FindAction("Move").canceled -= OnMove;
            _playerInputActionMap.FindAction("Jump").performed -= OnJump;
            _playerInputActionMap.FindAction("Jump").canceled -= OnJump;
        }
        
        // Setup jump variables based off the max jump height and time to the apex of the jump
        private void SetupJumpVariable()
        {
            float timeToApex = _maxJumpTime / 2;
            _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            InitialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        }

        // Actions to be taken when the player tries to move
        private void OnMove(InputAction.CallbackContext ctx)
        {
            MovementInput = ctx.ReadValue<Vector2>();
            IsMovementPressed = MovementInput.x != 0 || MovementInput.y != 0;
            _currentMovement = new Vector3(MovementInput.x, 0, MovementInput.y);
        }

        // Actions to be taken when the player tries to jump
        private void OnJump(InputAction.CallbackContext ctx)
        {
            IsJumpPressed = ctx.ReadValueAsButton();
        }

        // enable player actions and enter the first state
        private void Start()
        {
            //GameManager.Instance.EnablePlayerActions();
            CurrentState = _states.GetState<PlayerGroundedState>();
            CurrentState.EnterState();
        }
        
        private void Update()
        {
            CurrentState.UpdateStates();
            HandleMovement();
        }
        
        // Apply gravity based off the current root state
        public void HandleGravity()
        {
            float previousYVelocity = _currentMovement.y;

            switch (CurrentState.GetType())
            {
                case {} groundedState when groundedState == typeof(PlayerGroundedState):
                    _currentMovement.y = _groundedGravity;
                    _appliedMovement.y = _groundedGravity;
                    break;
                case {} jumpState when jumpState == typeof(PlayerJumpState):
                    _currentMovement.y += _gravity * Time.deltaTime;
                    _appliedMovement.y = (previousYVelocity + _currentMovement.y) * 0.5f;
                    break;
                case {} fallingState when fallingState == typeof(PlayerFallingState):
                    _currentMovement.y += _gravity * _fallSpeedMultiplier * Time.deltaTime;
                    _appliedMovement.y = (previousYVelocity + _currentMovement.y) * 0.5f;
                    break;
            }
            
            // Clamping fall speed to prevent player from falling too fast
            _currentMovement.y = Mathf.Max(_currentMovement.y, -_maxFallSpeed);
        }

        private void HandleMovement()
        {
            _appliedMovement.x = _currentMovement.x * _moveSpeed;
            _appliedMovement.z = _currentMovement.z * _moveSpeed;

            _characterController.Move(transform.TransformDirection(_appliedMovement * Time.deltaTime));

            AdjustPlayerFaceDirection();
        }

        private void AdjustPlayerFaceDirection()
        {
            // Get the Camera for this player and then lerp the direction the player is facing (around the Y axis) to the forward direction of the camera on the XZ plane
            Vector3 cameraXYForward = new Vector3(_playerCamera.transform.forward.x, 0, _playerCamera.transform.forward.z).normalized;
            
            if (cameraXYForward != Vector3.zero && IsMovementPressed)
            {
                transform.forward = Vector3.Lerp(transform.forward, cameraXYForward, Time.deltaTime * 10f);
            }
        }
    }
}
