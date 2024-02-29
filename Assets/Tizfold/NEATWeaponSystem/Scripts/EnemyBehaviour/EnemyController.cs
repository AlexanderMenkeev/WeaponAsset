using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour {
    public class EnemyController : MonoBehaviour, IDamagable
    {
        public float HealthPoints { get; set; }
        public float MovementSpeed;
        public float AttackDistance;
    
        public Rigidbody2D Rigidbody;
        public EnemyWeapon Weapon;
        public CircleCollider2D Collider;
        public SpriteRenderer Renderer;
        public GameObject Player;
    
        public EnemyStateMachine StateMachine;
    
        // Assigned from the Editor
        public GlobalVariablesSO GlobalVariables;
        public Projectile ProjectilePrefab;
        public Sprite DefaultSprite;
        public Sprite HurtSprite;
    
        private void Awake() {
            HealthPoints = 16f;
            MovementSpeed = 2.5f;
            AttackDistance = 5.5f;
        
            Rigidbody = GetComponent<Rigidbody2D>();
            Weapon = GetComponentInChildren<EnemyWeapon>();
            Collider = GetComponent<CircleCollider2D>();
            Renderer = GetComponent<SpriteRenderer>();
            Player = GameObject.Find("Player");
        
            StateMachine = new EnemyStateMachine(this);
        }
        private void Start() { StateMachine.Initialize(StateMachine.ChasingPlayer); }
        private void Update() { StateMachine.Update(); }

        
        private void FixedUpdate() {
            Vector2 playerDir = (Player.transform.position - transform.position).normalized;
            transform.up = playerDir;
            StateMachine.FixedUpdate();
        }
        private void LateUpdate() { StateMachine.LateUpdate(); }
    
        public void TakeDamage(float damage) {
            StateMachine.TransitionTo(StateMachine.Hurt);
            Rigidbody.velocity = Vector2.zero;
            HealthPoints -= damage;
        }
    
    
    
    
    }
}




