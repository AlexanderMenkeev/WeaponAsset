using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour.EnemyStates {
    public class DeadState : IState {
        private EnemyController _enemy;

        public DeadState(EnemyController enemy) {
            _enemy = enemy;
        }

        public void Enter() {
            _enemy.Rigidbody.velocity = Vector2.zero;
            Object.Destroy(_enemy.gameObject);
        }

        public void Update() { }

        public void FixedUpdate() { }

        public void LateUpdate() { }

        public void Exit() { }


    }
}