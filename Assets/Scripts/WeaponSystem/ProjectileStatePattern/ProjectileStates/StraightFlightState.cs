using UnityEngine;

namespace WeaponSystem.ProjectileStatePattern.ProjectileStates {
    public class InitialFlightState : IState {
        private Projectile _projectile;
        public float Radius;
        public InitialFlightState(Projectile projectile) {
            _projectile = projectile;
        }

        
        public void Enter() {
            _projectile.Rigidbody.velocity = _projectile.InitialVelocity;
            _projectile.transform.up = _projectile.Rigidbody.velocity;
            Radius = 0.2f * _projectile.WeaponParamsLocal.NNControlDistance;
            _projectile._spriteRenderer.color = Color.white;
        }

        public void Update() {
        }

        public void FixedUpdate() {
        }

        
        public void LateUpdate() {
            if (_projectile.DistanceFromOrigin > Radius)
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Controlled);

            if (_projectile._commonVariables.IsPaused && !_projectile.IsUI)
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Pause);


        }

        public void Exit() {
        }
    }
}