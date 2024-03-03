using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using Tizfold.NEATWeaponSystem.Scripts.Managers;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour.EnemyStates {
    public class HurtState : IState {

        private EnemyController _enemy;

        public HurtState(EnemyController enemy) {
            _enemy = enemy;
        }
        
        public void Enter() {
            _enemy.StartCoroutine(_enemy.StateMachine.TransitionTo(_enemy.StateMachine.Shooting, 0.2f));
            AudioManager.Instance.PlayAudioEffect(_enemy.AudioSource, AudioManager.Instance.EnemyHurt);
            
            _enemy.Renderer.material.shader = _enemy.ShaderGUItext;
            _enemy.Renderer.color = Color.white;
        }

        public void Update() { }

        public void FixedUpdate() { }

        public void LateUpdate() { }

        public void Exit() {
            _enemy.Renderer.material.shader = _enemy.ShaderSpritesDefault;
            _enemy.Renderer.color = Color.white;
        }
        
    }
}