using Tizfold.NEATWeaponSystem.Scripts.Interfaces;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour.EnemyStates {
    public class HurtState : IState {

        private EnemyController _enemy;

        public HurtState(EnemyController enemy) {
            _enemy = enemy;
        }
        
        public void Enter() {
            _enemy.StartCoroutine(_enemy.StateMachine.TransitionTo(_enemy.StateMachine.Shooting, 0.15f));
            _enemy.Renderer.sprite = _enemy.HurtSprite;
        }

        public void Update() { }

        public void FixedUpdate() { }

        public void LateUpdate() { }

        public void Exit() {
            _enemy.Renderer.sprite = _enemy.DefaultSprite;
        }
        
    }
}