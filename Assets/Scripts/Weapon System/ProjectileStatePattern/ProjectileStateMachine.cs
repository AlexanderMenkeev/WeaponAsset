using System;
using System.Collections;
using UnityEngine;

using ProjectileStates;
[Serializable]
public class ProjectileStateMachine {
    public IState CurrentState { get; private set; }
    public IState PreviousState { get; private set; }
    
    
    public StraightFlightState StraightFlight;
    public ControlledState Controlled;
    public PauseState Pause;
    public ReflectionState Reflection;
    
    public event Action<IState> StateChanged;
    public ProjectileStateMachine(Projectile projectile) {
        this.StraightFlight = new StraightFlightState(projectile);
        this.Controlled = new ControlledState(projectile);
        this.Pause = new PauseState(projectile);
        this.Reflection = new ReflectionState(projectile);
    }
    
    
    
    public void Initialize(IState initialState) {
        CurrentState = initialState;
        initialState.Enter();
        
        StateChanged?.Invoke(initialState);
    }
    
    public void TransitionTo(IState nextState) {
        CurrentState.Exit();
        PreviousState = CurrentState;
        CurrentState = nextState;
        nextState.Enter();
        
        StateChanged?.Invoke(nextState);
    }
    
    // coroutine for delayed transition
    public IEnumerator TransitionTo(IState nextState, float delay) {
        yield return new WaitForSeconds(delay);
        TransitionTo(nextState);
    }
    
    public void Update() {
        CurrentState?.Update();
    }

    public void FixedUpdate() {
        CurrentState?.FixedUpdate();
    }

    public void LateUpdate() {
        CurrentState?.LateUpdate();
    }
}
