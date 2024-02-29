using System.Collections;
using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using Tizfold.NEATWeaponSystem.Scripts.Managers;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour {
    public class EnemyProjectile : Projectile, IDamagable
    {
        [SerializeField] private GlobalVariablesSO _globalVariables;
        [SerializeField] private float _lifespan;
        private Transform _pivotPoint;
        public float Damage;
        public int LayerMask;
        public float HealthPoints { get; set; }
        private float _birthTime;
        
        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            _lifespan = 3f;
            _pivotPoint = transform.Find("PivotPoint");
            Damage = 1f;
            LayerMask = 0b_0000_0100_0000; // layer 6: Player
            HealthPoints = 5f;
            _birthTime = Time.time;
        }

        
        
        
    
        private void CheckCollision() {
            RaycastHit2D hit = Physics2D.Raycast(_pivotPoint.position, -transform.right, transform.localScale.x, LayerMask);

            if (ReferenceEquals(hit.collider, null))
                return;
        
            IDamagable objectToDamage = hit.transform.GetComponent<IDamagable>();
            LocalGameManager.Instance.DamageObject(objectToDamage, Damage);
            Debug.Log(objectToDamage);

        
            Destroy(gameObject);
        }
    
    
        public void TakeDamage(float damage) {
            HealthPoints -= damage;
            if (HealthPoints <= 0)
                Destroy(gameObject);
        }
    
    }
}
