using System;
using System.Collections;
using NeatProjectiles.Core.Scripts.Interfaces;
using NeatProjectiles.Core.Scripts.SODefinitions;
using NeatProjectiles.GameExample.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NeatProjectiles.GameExample.Scripts.Player {
    public class Player : MonoBehaviour, IDamagable
    {
        // assign in editor
        [SerializeField] private GlobalVariablesSO _globalVariables;
        
        [HideInInspector] [SerializeField] private Rigidbody2D _rigidbody;
        [HideInInspector] [SerializeField] private SpriteRenderer _renderer;
        [HideInInspector] [SerializeField] private GameWeapon _weapon;
        [HideInInspector] [SerializeField] private AudioSource _audioSource;

        [HideInInspector] [SerializeField] private Shader _shaderGUItext;
        [HideInInspector] [SerializeField] private Shader _shaderSpritesDefault;
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _weapon = FindObjectOfType<GameWeapon>();
            _audioSource = GetComponent<AudioSource>();
            
            _shaderGUItext = Shader.Find("GUI/Text Shader");
            _shaderSpritesDefault = _renderer.material.shader;
        }

        
        private InputManager _inputManager;
        private InputAction _moveAction;
        private InputAction _aimAction;
    
        [field: SerializeField] [field: Range(100f, 1000f)] public float HealthPoints { get; set; }
        [field: SerializeField] [field: Range(3f, 5.5f)] public float Speed { get; set; }
        private void Initialize() {
            _inputManager = InputManager.Instance;
            _moveAction = _inputManager.PlayerActionMap.moveAction;
            _aimAction = _inputManager.PlayerActionMap.aimAction;

            HealthPoints = 1000f;
            Speed = 3.5f;
            _currPos = transform.position;
        }
        
        
        private void Start() {
            Initialize();
            
            _aimAction.started += StartShooting;
            _aimAction.canceled += StopShooting;
            _globalVariables.OnPauseResumeEvent += OnPauseResumeGame;
        }

        private void OnDestroy() {
            _aimAction.started -= StartShooting;
            _aimAction.canceled -= StopShooting;
            _globalVariables.OnPauseResumeEvent -= OnPauseResumeGame;
        }
        
        private void StartShooting(InputAction.CallbackContext context) {
            _shootCoroutine = StartCoroutine(Shoot());
        }
        
        private void StopShooting(InputAction.CallbackContext context) {
            StopCoroutine(_shootCoroutine);
        }

        [SerializeField] [Range(0.1f, 2f)] private float _fireRate = 1f;
        private Coroutine _shootCoroutine;
        private IEnumerator Shoot() {
            while (true) {
                yield return new WaitForSeconds(_fireRate * 0.1f);
                AudioManager.Instance.PlayAudioEffect(_audioSource, AudioManager.Instance.PlayerShoot);
                _weapon.FireMultiShot();
                yield return new WaitForSeconds(_fireRate * 0.9f);
            }
        }
        
        private void OnPauseResumeGame() {
            _rigidbody.velocity = Vector2.zero;
        }


        private InputControl _moveControl;
        public Vector2 Move { get; private set; }
        public Vector2 Aim { get; private set; }
        private void Update() {
            _moveControl = _moveAction.activeControl;
            Move = _moveAction.ReadValue<Vector2>();
            Aim = _aimAction.ReadValue<Vector2>();
        }
        
    
        private Vector2 _prevPos;
        private Vector2 _currPos;
        [HideInInspector] public Vector2 ActualVelocity;
        private void FixedUpdate() {
            if (_globalVariables.IsPaused)
                return;
            
            MoveFunc();
            
            if (Aim != Vector2.zero) 
                _weapon.transform.up = Aim;
            
            
            if (HealthPoints <= 0)
                OnDeath();
            
            // ActualVelocity is needed for MapReposition
            // Using rigidbody.velocity is not reliable
            _prevPos = _currPos;
            _currPos = transform.position;
            ActualVelocity = (_currPos - _prevPos) / Time.fixedDeltaTime;
        }

        
        
        private void MoveFunc() {
            if (_moveControl == null) {
                _rigidbody.velocity -= _rigidbody.velocity * (0.3f * Time.deltaTime);
                return;
            }
            
            switch (_moveControl.device.name) {
                case "Keyboard":
                    if (Move.y > 0)
                        _rigidbody.velocity += (Vector2)transform.up * (Move.y * 100f * Time.deltaTime);
                    else 
                        _rigidbody.velocity += _rigidbody.velocity * (Move.y * 10f * Time.deltaTime);

                    if (Move.y != 0f) {
                        Quaternion rotation = Move.x > 0
                            ? Quaternion.Euler(0f, 0f, -200f * Time.deltaTime)
                            : Quaternion.Euler(0f, 0f, 200f * Time.deltaTime);
                        
                        if (Move.x != 0f)
                            transform.rotation *= rotation;
                    }
                    
                    break;
                
                case "Gamepad":
                    _rigidbody.velocity += Move * (100f * Time.deltaTime);
                    
                    float angleDeg = Vector3.SignedAngle(Vector3.up, _rigidbody.velocity, Vector3.forward);
                    transform.rotation = Quaternion.AngleAxis( angleDeg, Vector3.forward );
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Limit max speed
            float speed = _rigidbody.velocity.magnitude;
            if (speed > Speed)
                _rigidbody.velocity = _rigidbody.velocity.normalized * Speed;
        }

    
        
        private void OnDrawGizmos() {
            Gizmos.DrawRay(gameObject.transform.position, Aim * 10f);
        }
        
        private IEnumerator ChangeShader(Shader shader, float delay) {
            yield return new WaitForSeconds(delay);
            _renderer.material.shader = shader;
            _renderer.color = Color.white;
        }
        
        public void TakeDamage(float damage) {
            StartCoroutine(ChangeShader(_shaderGUItext,0f));
            AudioManager.Instance.PlayAudioEffect(_audioSource, AudioManager.Instance.PlayerHurt);
            HealthPoints -= damage;
            StartCoroutine(ChangeShader(_shaderSpritesDefault,0.2f));
        }

        private void OnDeath() {
            GameManager.Instance.ActivateDeathCanvas();
        }
        
        
    
    }
}
