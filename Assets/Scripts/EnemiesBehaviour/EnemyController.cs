using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyController : MonoBehaviour, IDamagable
{
    public float HealthPoints { get; set; }
    public float MovementSpeed;
    public float AttackDistance;
    
    public Rigidbody2D Rigidbody;
    public Animator Animator;
    public EnemyWeapon Weapon;
    public SpriteRenderer SpriteRenderer;
    public CapsuleCollider2D Collider;
    public GameObject Player;
    
    public EnemyStateMachine StateMachine;
    
    // Assigned from the Editor
    public CommonVariablesSO CommonVariables;
    public EnemyProjectile ProjectilePrefab;
    
    private void Awake() {
        HealthPoints = 16f;
        MovementSpeed = 2f;
        AttackDistance = 5.5f;
        
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Weapon = GetComponentInChildren<EnemyWeapon>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<CapsuleCollider2D>();
        Player = GameObject.Find("Player");
        
        StateMachine = new EnemyStateMachine(this);
    }
    private void Start() {
        StateMachine.Initialize(StateMachine.Awake);
    }
    private void Update() {
        StateMachine.Update();
    }
    private void FixedUpdate() {
        StateMachine.FixedUpdate();
    }
    private void LateUpdate() {
        StateMachine.LateUpdate();
        
    }
    
    public void TakeDamage(float damage) {
        StateMachine.TransitionTo(StateMachine.Hurt);
        Rigidbody.velocity = Vector2.zero;
        HealthPoints -= damage;
    }
    
    
    
    
}




