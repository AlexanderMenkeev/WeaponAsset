using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using WeaponSystem.ProjectileStatePattern.ProjectileStates.Common;
using WeaponSystem.ProjectileStatePattern.ProjectileStates.DefaultMode;

namespace WeaponSystem.ProjectileStatePattern {
    [Serializable]
    public class ProjectileStateMachine {
        public IState CurrentState { get; private set; }
        public IState PreviousState { get; private set; }
        
        public InitialFlightState InitialFlight;
        public ControlledState Controlled;
        public PauseState Pause;
        public ReflectionState Reflection;
    
        public event Action<IState> StateChanged;
        public ProjectileStateMachine(Projectile projectile) {
            InitialFlight = new InitialFlightState(projectile);
            Controlled = new ControlledState(projectile);
            Pause = new PauseState(projectile);
            Reflection = new ReflectionState(projectile);
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
}
