using Interfaces;
using UnityEngine;

namespace WeaponSystem.ProjectileStatePattern.ProjectileStates.Common {
    public class InitialFlightState : IState {
        private Projectile _projectile;
        public float InitialFlightRadius;
        public InitialFlightState(Projectile projectile) {
            _projectile = projectile;
        }

        
        public void Enter() {
            _projectile.Rigidbody.velocity = _projectile.InitialVelocity;
            _projectile.transform.up = _projectile.Rigidbody.velocity;
            InitialFlightRadius = _projectile.WeaponParamsLocal.NNControlDistance * _projectile.WeaponParamsLocal.InitialFlightRadius;
            _projectile.SpriteRenderer.color = Color.white;
        }

        public void Update() { }

        public void FixedUpdate() {
        }

        
        public void LateUpdate() {
            if (_projectile.DistanceFromOrigin > InitialFlightRadius)
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Controlled);

            if (_projectile.GlobalVariables.IsPaused)
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Pause);


        }

        public void Exit() {
        }
    }
}