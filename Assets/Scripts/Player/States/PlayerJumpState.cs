using UnityEngine;

namespace Player.States
{
    public class PlayerJumpState : BaseState
    {
        // reference to the PlayerStateMachine to limit casting to it from the baseState StateMachine ctx variable
        private readonly PlayerStateMachine _playerStateMachine;
        
        public PlayerJumpState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
        {
            _isRootState = true;
            _playerStateMachine = currentContext as PlayerStateMachine;
        }

        public override void EnterState()
        {
            _playerStateMachine.IsJumping = true;
            HandleJump();
            // Todo: Add animation value change
        }

        protected override void UpdateState()
        {
            _playerStateMachine.HandleGravity();

            CheckSwitchStates();
        }

        public override void ExitState()
        {
            _playerStateMachine.IsJumping = false;
            // Todo: Add animation value change
        }

        protected override void CheckSwitchStates()
        {
            if (_playerStateMachine._currentMovement.y <= 0f || !_playerStateMachine.IsJumpPressed)
            {
                SwitchState(_factory.GetState<PlayerFallingState>());
            }
        }

        public override void InitializeSubState()
        {
            
        }
        
        private void HandleJump()
        {
            _playerStateMachine._currentMovement.y += _playerStateMachine.InitialJumpVelocity;
            _playerStateMachine._appliedMovement.y += _playerStateMachine.InitialJumpVelocity;
        }
    }
}