namespace Player.States
{
    public class PlayerFallingState : BaseState
    {
        // reference to the PlayerStateMachine to limit casting to it from the baseState StateMachine ctx variable
        private readonly PlayerStateMachine _playerStateMachine;


        public PlayerFallingState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
        {
            _isRootState = true;
            _playerStateMachine = currentContext as PlayerStateMachine;
        }

        public override void EnterState()
        {
            // ToDo: Set animation value
        }

        protected override void UpdateState()
        {
            _playerStateMachine.HandleGravity();
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            // ToDo: Set animation value
        }

        protected override void CheckSwitchStates()
        {
            if (_playerStateMachine._characterController.isGrounded)
            {
                SwitchState(_factory.GetState<PlayerGroundedState>());
            }
        }

        public override void InitializeSubState()
        {
            
        }
    }
}