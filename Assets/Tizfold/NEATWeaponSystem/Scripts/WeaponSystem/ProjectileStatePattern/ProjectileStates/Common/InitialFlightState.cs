using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern.ProjectileStates.Common {
    
    public class InitialFlightState : IState {
        
        private Projectile _projectile;
        public InitialFlightState(Projectile projectile) {
            _projectile = projectile;
        }

        private float _initialFlightRadius;
        private WeaponParams _weaponParams;
        public void Enter() {
            _weaponParams = _projectile.WeaponParamsLocal;
            
            _projectile.Rigidbody.velocity = _projectile.InitialVelocity;
            _projectile.transform.up =  _projectile.InitialVelocity;
            
            _initialFlightRadius = _weaponParams.NNControlDistance * _weaponParams.InitialFlightRadius;
            _projectile.SpriteRenderer.color = Color.white;
        }

        public void Update() { }

        public void FixedUpdate() { }
        
        public void LateUpdate() {
            if (_projectile.DistanceFromOrigin > _initialFlightRadius)
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Controlled);
        }

        public void Exit() { }
    }
}