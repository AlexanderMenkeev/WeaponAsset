using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectileStates {
    public class ReflectionState : IState {
        private Projectile _projectile;
        
        
        public ReflectionState(Projectile projectile) {
            _projectile = projectile;
        }

        
        public void Enter() {
            _projectile._spriteRenderer.color = Color.white;
            _reflected = false;
        }
        
        public void Update() {
            
        }
        
        
        private Vector2 _normal;
        private bool _reflected;
        public void FixedUpdate() {
            _normal = (_projectile.ParentTransform.position - _projectile.transform.position).normalized;
            
            if (_projectile.DistanceFromOrigin / _projectile.NeuralNetworkControlDistance > _projectile.ReflectiveCircleRadius && !_reflected) {
                _projectile.Rigidbody.velocity = Vector2.Reflect(_projectile.Rigidbody.velocity, _normal);
                _reflected = true;
            }
            
            if (_projectile.DistanceFromOrigin / _projectile.NeuralNetworkControlDistance < _projectile.ReflectiveCircleRadius)
                _reflected = false;
        }

        
        public void LateUpdate() {
            if (_projectile._commonVariables.IsPaused && !_projectile.IsUI)
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Pause);
            
            if (_projectile.DistanceFromOrigin / _projectile.NeuralNetworkControlDistance <= math.SQRT2) 
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Controlled);
            
        }

        public void Exit() {
        }
    }
}
