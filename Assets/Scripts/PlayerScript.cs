using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerScript : MonoBehaviour, IDamagable
{
    [SerializeField] public Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private InputAction _moveAction;
    private InputAction _aimAction;

    [SerializeField] private float _speed;

    private GameSceneWeapon _weapon;
    private InputManager _inputManager;
    
    [SerializeField] private CommonVariablesSO _commonVariables;
    
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
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _weapon = GetComponentInChildren<GameSceneWeapon>();
    }
    
    
    public void OnPauseResumeGame() {
        _rigidbody.velocity = Vector2.zero;
        _animator.enabled = !_commonVariables.IsPaused;
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
        Move = _moveAction.ReadValue<Vector2>();
        Aim = _aimAction.ReadValue<Vector2>();
    }
    
    

    private void StartShooting(InputAction.CallbackContext context)
    {
        Debug.Log("started shooting");
        _weapon.FireCoroutine = StartCoroutine(_weapon.FireProjectile());
    }
    private void StopShooting(InputAction.CallbackContext context)
    {
        Debug.Log("stopped shooting");
        StopCoroutine(_weapon.FireCoroutine);
    }
    
    public Vector3 ActualVelocity;
    private Vector3 _lastPos;
    private void FixedUpdate() {
        if (_commonVariables.IsPaused)
            return;
        _rigidbody.velocity = Move * _speed;
        _animator.SetFloat("speed", _rigidbody.velocity.magnitude);

        if (Aim != Vector2.zero) {
            _weapon.transform.right = Aim;
            
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
