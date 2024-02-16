using Interfaces;
using Unity.Mathematics;

namespace WeaponSystem.ProjectileStatePattern.ProjectileStates {
    public class ControlledState : IState {
        private Projectile _projectile;

        public ControlledState(Projectile projectile) {
            _projectile = projectile;
        }
        
        public void Enter() { }
        public void Update() { }

        public void FixedUpdate() {
            _projectile.ActivateBlackBox();
            _projectile.ReadDataFromBlackBox();
            _projectile.LimitMaxSpeed();
            _projectile.DestroyYourself();
        }
        
        public void LateUpdate() {
            if (_projectile.GlobalVariables.IsPaused) 
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Pause);
            
            if (_projectile.DistanceFromOrigin / _projectile.WeaponParamsLocal.NNControlDistance > math.SQRT2) 
                _projectile.StateMachine.TransitionTo(_projectile.StateMachine.Reflection);
        }
        
        
        public void Exit() {
        }
        
        
    }
}