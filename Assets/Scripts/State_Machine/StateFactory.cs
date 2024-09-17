using System;
using System.Reflection;

public class StateFactory
{
    private StateMachine _contex;
    
    // Constructor to register the context this factory being used by
    public StateFactory(StateMachine currentContext)
    {
        _contex = currentContext;
    }

    // This is used to create all the states.
    // its used whenever you wish to change states
    public BaseState GetState<TState>() where TState : BaseState
    {
        // Check if TState has the Constructor for class BaseState
        ConstructorInfo constructorInfo = typeof(TState).GetConstructor(new[] { typeof(StateMachine), typeof(StateFactory) });

        // If TState does not have the Constructor throw an exception
        if (constructorInfo == null) throw new InvalidOperationException($"type of {typeof(TState)} does not have the constructor for BaseState");
        
        // If TState does have the Constructor invoke the constructor and return the instance
        TState instance = (TState)constructorInfo.Invoke(new object[] { _contex, this });
        return instance;
    }
}