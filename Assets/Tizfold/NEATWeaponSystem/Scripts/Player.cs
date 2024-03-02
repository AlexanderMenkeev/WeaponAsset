using System;
using System.IO;
using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using Tizfold.NEATWeaponSystem.Scripts.Managers;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tizfold.NEATWeaponSystem.Scripts {
    public class Player : MonoBehaviour, IDamagable
    {
        // assign in editor
        [SerializeField] private GlobalVariablesSO _globalVariables;
        
        
        [SerializeField] private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;
        private GameWeapon _weapon;
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _weapon = GetComponentInChildren<GameWeapon>();
            
            
        }

        
        private InputManager _inputManager;
        private InputAction _moveAction;
        private InputAction _aimAction;
    
        public float HealthPoints { get; set; }
        [SerializeField] private float _speed;
        
        private void Initialize() {
            _inputManager = InputManager.Instance;
            _moveAction = _inputManager.PlayerActionMap.moveAction;
            _aimAction = _inputManager.PlayerActionMap.aimAction;

            HealthPoints = 10f;
            _speed = 4f;
        }
    
    
        public void TakeDamage(float damage) {
            HealthPoints -= damage;
        }
    

        private void Start() {
            Initialize();
        
            _aimAction.started += StartShooting;
            _aimAction.canceled += StopShooting;
            _globalVariables.OnPauseResumeEvent += OnPauseResumeGame;
            _currPos = transform.position;
        }

        private void OnDestroy() {
            _aimAction.started -= StartShooting;
            _aimAction.canceled -= StopShooting;
            _globalVariables.OnPauseResumeEvent -= OnPauseResumeGame;
        }
        
        private void StartShooting(InputAction.CallbackContext context) {
            _weapon.FireCoroutine = StartCoroutine(_weapon.Fire());
        }
        
        private void StopShooting(InputAction.CallbackContext context) {
            StopCoroutine(_weapon.FireCoroutine);
        }
        
        private void OnPauseResumeGame() {
            _rigidbody.velocity = Vector2.zero;
        }

        public Vector2 Move { get; private set; }
        public Vector2 Aim { get; private set; }
        private void Update() {
            Move = _moveAction.ReadValue<Vector2>().normalized;
            Aim = _aimAction.ReadValue<Vector2>().normalized;
        }
        
    
        private Vector2 _prevPos;
        private Vector2 _currPos;
        public Vector2 ActualVelocity;
        private void FixedUpdate() {
            if (_globalVariables.IsPaused)
                return;

            _rigidbody.velocity = Move * _speed;
            
            float angleDeg = Vector3.SignedAngle(Vector3.up, Move, Vector3.forward);

            // For some reason <tranform.up = Move;> did not work correctly.
            // Quaternions came to the rescue!
            if (Move != Vector2.zero) 
                transform.rotation = Quaternion.AngleAxis( angleDeg, Vector3.forward );
            
            if (Aim != Vector2.zero) 
                _weapon.transform.up = Aim;
            
        
            // ActualVelocity is needed for MapReposition
            // Using rigidbody.velocity is not reliable
            _prevPos = _currPos;
            _currPos = transform.position;
            ActualVelocity = (_currPos - _prevPos) / Time.fixedDeltaTime;
        }

    
        private void OnDrawGizmos() {
            Gizmos.DrawRay(gameObject.transform.position, Aim * 10f);
        }
    
    }
}
