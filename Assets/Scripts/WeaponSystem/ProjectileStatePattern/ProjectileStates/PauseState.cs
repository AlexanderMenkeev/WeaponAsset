using Interfaces;
using UnityEngine;

namespace WeaponSystem.ProjectileStatePattern.ProjectileStates {
    
public class PauseState : IState {
    private Projectile _projectile;
    
    public PauseState(Projectile projectile) {
        _projectile = projectile;
    }


    private Vector2 _prevVelocity;
    public void Enter() {
        _prevVelocity = _projectile.ActualVelocity;
        _projectile.Rigidbody.velocity = Vector2.zero;
    }

    public void Update() {
    }

    public void FixedUpdate() {
    }

    
    public void LateUpdate() {
        if (!_projectile.GlobalVariables.IsPaused)
            _projectile.StateMachine.TransitionTo(_projectile.StateMachine.PreviousState);
    }

    public void Exit() {
        _projectile.Rigidbody.velocity = _prevVelocity;
    }
}

}