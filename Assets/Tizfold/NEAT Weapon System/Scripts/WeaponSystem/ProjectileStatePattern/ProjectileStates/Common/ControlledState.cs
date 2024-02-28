using System;
using System.Linq.Expressions;
using Interfaces;
using SODefinitions;
using Unity.Mathematics;
using UnityEngine;
using WeaponSystem.Weapon;

namespace WeaponSystem.ProjectileStatePattern.ProjectileStates.Common {
    
    public class ControlledState : IState {
        
        private Projectile _projectile;

        public ControlledState(Projectile projectile) {
            _projectile = projectile;
        }
        
        private WeaponParams _weaponParams;
        public void Enter() {
            _weaponParams = _projectile.WeaponParamsLocal;
        }
        
        public void Update() { }

        public void FixedUpdate() {
            switch (_weaponParams.Mode) {
                case ProjectileMode.CircleReflection:
                    _projectile.ActivateBlackBoxCircle();
                    break;
                case ProjectileMode.RectangleReflection:
                    _projectile.ActivateBlackBoxRect();
                    break;
                case ProjectileMode.Polar:
                    _projectile.ActivateBlackBoxPolar();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _projectile.ReadDataFromBlackBox();
            _projectile.LimitMaxSpeed();
        }
        
        public void LateUpdate() {

            switch (_weaponParams.Mode) {
                case ProjectileMode.CircleReflection:
                    CircleModeTransitions();
                    break;
                case ProjectileMode.RectangleReflection:
                    RectModeTransitions();
                    break;
                case ProjectileMode.Polar:
                    PolarModeTransitions();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Exit() { }
        
        private void CircleModeTransitions() {
            if (_projectile.DistanceFromOrigin > _weaponParams.NNControlDistance * math.SQRT2) 
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.CircleReflection);
        }
        
        private void RectModeTransitions() {
            float maxX = _weaponParams.RectDimensions.x * _weaponParams.NNControlDistance;
            float maxY = _weaponParams.RectDimensions.y * _weaponParams.NNControlDistance;
            
            if (Mathf.Abs(_projectile.RelativePos.x) > maxX || 
                Mathf.Abs(_projectile.RelativePos.y) > maxY) 
                
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.RectReflection);
        }

        private void PolarModeTransitions() {
            if (_projectile.PhiRad > _weaponParams.MaxPolarAngleDeg * Mathf.Deg2Rad || 
                _projectile.DistanceFromOrigin < _weaponParams.NNControlDistance * _weaponParams.InitialFlightRadius)
                
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.ConeReflection);
        }
        
        
    }
}