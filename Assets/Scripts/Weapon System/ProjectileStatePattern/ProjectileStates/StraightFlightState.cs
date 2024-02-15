using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileStates {
    public class StraightFlightState : IState {
        private Projectile _projectile;
        public float Radius;
        public StraightFlightState(Projectile projectile) {
            _projectile = projectile;
        }

        
        public void Enter() {
            _projectile.Rigidbody.velocity = _projectile.InitialVelocity;
            _projectile.transform.up = _projectile.Rigidbody.velocity;
            Radius = 0.2f * _projectile.NeuralNetworkControlDistance;
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