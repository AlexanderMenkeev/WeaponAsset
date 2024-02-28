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
        [SerializeField] public Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private InputAction _moveAction;
        private InputAction _aimAction;

        [SerializeField] private float _speed;

        //private GameSceneWeapon _weapon;
        private InputManager _inputManager;
    
        [SerializeField] private GlobalVariablesSO _commonVariables;
    
        public float HealthPoints { get; set; }
        public void TakeDamage(float damage) {
            HealthPoints -= damage;
        }

        private void Initialize()
        {
            _inputManager = InputManager.Instance;
            _moveAction = _inputManager.PlayerActionMap.moveAction;
            _aimAction = _inputManager.PlayerActionMap.aimAction;

            HealthPoints = 10f;
        }
    
        private void Awake() {
        
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        
            //_weapon = GetComponentInChildren<GameSceneWeapon>();
        }
    
    
        public void OnPauseResumeGame() {
            _rigidbody.velocity = Vector2.zero;
        }

        private void Start() {
            Initialize();
        
            _speed = 3f;
            _aimAction.started += StartShooting;
            _aimAction.canceled += StopShooting;
            _commonVariables.OnPauseResumeEvent += OnPauseResumeGame;
            _lastPos = transform.position;
        }

        private void OnDestroy() {
            _aimAction.started -= StartShooting;
            _aimAction.canceled -= StopShooting;
            _commonVariables.OnPauseResumeEvent -= OnPauseResumeGame;
        }

        public Vector2 Move { get; private set; }
        public Vector2 Aim { get; private set; }
        private void Update() {
            Move = _moveAction.ReadValue<Vector2>().normalized;
            Aim = _aimAction.ReadValue<Vector2>().normalized;
        }
    
    

        private void StartShooting(InputAction.CallbackContext context)
        {
            Debug.Log("started shooting");
            //_weapon.FireCoroutine = StartCoroutine(_weapon.FireProjectile());
        }
        private void StopShooting(InputAction.CallbackContext context)
        {
            Debug.Log("stopped shooting");
            //StopCoroutine(_weapon.FireCoroutine);
        }
    
        public Vector3 ActualVelocity;
        private Vector3 _lastPos;
        private void FixedUpdate() {
            if (_commonVariables.IsPaused)
                return;

            _rigidbody.velocity = Move * _speed;
            
            float angleDeg = Vector3.Angle(Vector3.up, Move);
            if (Move.x <= 0f) {
                transform.rotation = Quaternion.AngleAxis( angleDeg, Vector3.forward );
            }
            else {
                transform.rotation = Quaternion.AngleAxis( -angleDeg, Vector3.forward );
            }
            
            
            
            // Debug.Log(_rigidbody.velocity);
            // Quaternion.AngleAxis(1f, Vector3.forward);
            // if (_rigidbody.velocity != Vector2.zero)
            //     transform.rotation *= Quaternion.AngleAxis(1f, Vector3.forward);;
            //
            //transform.up = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, 0f);
           // transform.up = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
           //.
           // transform.up = Vector3.RotateTowards(transform.up, _rigidbody.velocity, math.PI, 1f);
            if (Aim != Vector2.zero) {
                //_weapon.transform.right = Aim;
            
            }
        
            ActualVelocity = (transform.position - _lastPos);
            _lastPos = transform.position;
        
        
        }

    
        private void OnDrawGizmos() {
            Gizmos.DrawRay(gameObject.transform.position, Aim * 10f);
        }


        private void LateUpdate() {
            if (Move.x != 0) {
                _spriteRenderer.flipX = Move.x < 0;
            } else if (Aim != Vector2.zero) {
                _spriteRenderer.flipX = Aim.x < 0;
            }
       
        }
    
    }
}
