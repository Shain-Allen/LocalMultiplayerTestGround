namespace Player.States
{
    public class PlayerWalkState : BaseState
    {
        // reference to the PlayerStateMachine to limit casting to it from the baseState StateMachine ctx variable
        private readonly PlayerStateMachine _playerStateMachine;
        
        public PlayerWalkState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
        {
            _isRootState = false;
            _playerStateMachine = currentContext as PlayerStateMachine;
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }

        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }
    }
}