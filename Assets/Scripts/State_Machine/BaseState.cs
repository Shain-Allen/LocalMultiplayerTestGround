// All States are derived from this, it defines the basic behaviour of a state
public abstract class BaseState
{
    // is this a base/upper most level state
    protected bool _isRootState = false;
    
    // reference to the context/State Machine that this state belongs to
    protected virtual StateMachine _ctx { get; private set; }
    
    // reference to the factory that creates states for the context/State Machine
    protected StateFactory _factory { get; private set; }
    
    // reference to the states "parent"
    protected BaseState _currentSuperState;
    
    // reference to the states "child"
    protected BaseState _currentSubState;


    // Constructor for a state, this takes in the StateMachine to use as a context and the StateFactory the context uses
    public BaseState(StateMachine currentContext, StateFactory stateFactory)
    {
        _ctx = currentContext;
        _factory = stateFactory;
    }
    
    // This is called once when the state is changed to. think of it like awake or start for the state
    public abstract void EnterState();

    // This is called every frame while the state is the current state of the context. called in UpdateStates
    protected abstract void UpdateState();

    // This is called once when the context's current state is changed. think of this like OnDestroy or OnDisable for the state
    public abstract void ExitState();

    // This is Called every frame while the state is the current state of the context and defines logic for when to switch to another state
    protected abstract void CheckSwitchStates();
    
    // This is called when the state is Initialized. Use this to initialize any substates
    public abstract void InitializeSubState();

    // This is called by the context every frame and updates both the itself and any substates it may have
    public void UpdateStates()
    {
        UpdateState();
        _currentSubState?.UpdateStates();
    }
    
    // You call this to switch states when checking in CheckSwitchStates if you need to change states
    // the Factory has a method called GetState that you can use to pass in the state you wish to switch to
    protected void SwitchState(BaseState newState)
    {
        // Current State Exits State
        ExitState();
        
        // New state enters state
        newState.EnterState();

        if (_isRootState)
        {
            // switch current state of context
            _ctx.CurrentState = newState;
        }
        else
        {
            _currentSuperState?.SetSubState(newState);
        }
    }
    
    // This is used to connect the current state to its parent/SuperState
    protected void SetSuperState(BaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    // This is used to connect the current state to its child/substate
    protected void SetSubState(BaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
