using System;
using System.Collections;
using System.Collections.Generic;
using SODefinitions;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyProjectile : MonoBehaviour, IDamagable
{
    public Rigidbody2D Rigidbody;
    [SerializeField] private CommonVariablesSO _commonVariables;
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

    
    public Vector2 Direction;
    public float ProjectileSpeed;
    private void Start() {
        _destroyItselfCoroutine = StartCoroutine(DestroyItself(_lifespan));
        Rigidbody.velocity = Direction * ProjectileSpeed;
        _commonVariables.OnPauseResumeEvent += OnPauseResumeGame;
    }

    private void OnDestroy() {
        _commonVariables.OnPauseResumeEvent -= OnPauseResumeGame;
    }
    

    private void FixedUpdate() {
        CheckCollision();
    }
    
    private void OnPauseResumeGame() {
        switch (_commonVariables.IsPaused) {
            case true:
                Rigidbody.velocity = Vector2.zero;
                StopCoroutine(_destroyItselfCoroutine);
                break;
            
            case false:
                float remainedLifespan = _lifespan - (_commonVariables.PauseTime - _birthTime);
                StartCoroutine(DestroyItself(remainedLifespan));
                Rigidbody.velocity = Direction * ProjectileSpeed;
                _birthTime = Time.time;
                break;
        }
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
    
    private Coroutine _destroyItselfCoroutine;
    private IEnumerator DestroyItself(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    
    
    public void TakeDamage(float damage) {
        HealthPoints -= damage;
        if (HealthPoints <= 0)
            Destroy(gameObject);
    }
    
}
