using System.Collections;
using NEATProjectiles.Core.Scripts.Interfaces;
using NEATProjectiles.Core.Scripts.SODefinitions;
using NEATProjectiles.Demos.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NEATProjectiles.Demos.Scripts.Player {
    public class Player : MonoBehaviour, IDamagable
    {
        // assign in editor
        [SerializeField] private GlobalVariablesSO _globalVariables;
        
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private GameWeapon _weapon;
        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private Shader _shaderGUItext;
        [SerializeField] private Shader _shaderSpritesDefault;
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _weapon = GetComponentInChildren<GameWeapon>();
            _audioSource = GetComponent<AudioSource>();
            
            _shaderGUItext = Shader.Find("GUI/Text Shader");
            _shaderSpritesDefault = _renderer.material.shader;
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

            HealthPoints = 100f;
            _speed = 4f;
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
            _shootCoroutine = StartCoroutine(Shoot());
        }
        
        private void StopShooting(InputAction.CallbackContext context) {
            StopCoroutine(_shootCoroutine);
        }

        private float _fireRate = 1f;
        private Coroutine _shootCoroutine;
        private IEnumerator Shoot() {
            yield return new WaitForSeconds(_fireRate * 0.1f);
            while (true) {
                AudioManager.Instance.PlayAudioEffect(_audioSource, AudioManager.Instance.PlayerShoot);
                _weapon.FireMultiShot();
                yield return new WaitForSeconds(_fireRate);
            }
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
            if (Move != Vector2.zero) 
                transform.rotation = Quaternion.AngleAxis( angleDeg, Vector3.forward );
            
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
        }
        
        
    
    }
}
