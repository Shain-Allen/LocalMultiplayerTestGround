namespace Player.States
{
    public class PlayerGroundedState : BaseState
    {
        // reference to the PlayerStateMachine to limit casting to it from the baseState StateMachine ctx variable
        private readonly PlayerStateMachine _playerStateMachine;
        
        public PlayerGroundedState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
        {
            _isRootState = true;
            _playerStateMachine = currentContext as PlayerStateMachine;
        }
        
        public override void EnterState()
        {
            // Todo: Add animation value change
        }

        protected override void UpdateState()
        {
            _playerStateMachine.HandleGravity();
            
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            // Todo: Add animation value change
        }

        protected override void CheckSwitchStates()
        {
            if (_playerStateMachine.IsJumpPressed && !_playerStateMachine.IsJumping)
            {
                SwitchState(_factory.GetState<PlayerJumpState>());
            }
            else if (!_playerStateMachine._characterController.isGrounded)
            {
                SwitchState(_factory.GetState<PlayerFallingState>());
            }
        }

        public override void InitializeSubState()
        {
            
        }
    }
}