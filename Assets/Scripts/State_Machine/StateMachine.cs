using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    // Reference to the StateFactory that creates the references to the states this StateMachine uses
    protected StateFactory _states;
    
    // The current state of the StateMachine
    public BaseState CurrentState { get; set; }
}
