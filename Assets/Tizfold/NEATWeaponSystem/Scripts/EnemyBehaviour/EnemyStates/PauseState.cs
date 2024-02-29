using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour.EnemyStates {
    public class PauseState : IState {

        private EnemyController _enemy;

        public PauseState(EnemyController enemy) {
            _enemy = enemy;
        }

        public void Enter() {
            _enemy.Rigidbody.velocity = Vector2.zero;
        }

        public void Update() { }

        public void FixedUpdate() { }

        public void LateUpdate() {
            if (_enemy.GlobalVariables.IsPaused == false)
                _enemy.StateMachine.TransitionTo(_enemy.StateMachine.ChasingPlayer);
        }

        public void Exit() { }
    }
}