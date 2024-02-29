using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour.EnemyStates {

    public class ChasingPlayerState : IState {
        private EnemyController _enemy;

        public ChasingPlayerState(EnemyController enemy) {
            _enemy = enemy;
        }

        public void Enter() { }

        public void Update() { }

        public void FixedUpdate() { ChasePlayer(); }

        private float _distanceToPlayer;
        public void LateUpdate() {
            if (_enemy.GlobalVariables.IsPaused)
                _enemy.StateMachine.TransitionTo(_enemy.StateMachine.Pause);

            if (_enemy.HealthPoints <= 0)
                _enemy.StateMachine.TransitionTo(_enemy.StateMachine.Dead);

            _distanceToPlayer = Vector2.Distance(_enemy.Player.transform.position, _enemy.transform.position);
            if (_distanceToPlayer < _enemy.AttackDistance)
                _enemy.StateMachine.TransitionTo(_enemy.StateMachine.Shooting);
        }

        public void Exit() { _enemy.Rigidbody.velocity = Vector2.zero; }
        
        private Vector2 _moveVec;
        private void ChasePlayer() {
            _moveVec = _enemy.Player.transform.position - _enemy.transform.position;
            _moveVec = _moveVec.normalized * _enemy.MovementSpeed;
            _enemy.Rigidbody.velocity = _moveVec;
        }

    }
}