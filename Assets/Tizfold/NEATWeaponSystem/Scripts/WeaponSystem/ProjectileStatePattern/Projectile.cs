using System;
using System.Collections;
using SharpNeat.Phenomes;
using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using Tizfold.NEATWeaponSystem.Scripts.Managers;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon;
using Unity.Mathematics;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern {
    public class Projectile : MonoBehaviour, IDamagable
    {
        // assign in editor
        [SerializeField] public GlobalVariablesSO GlobalVariables;
        
        // assigned from Abstract Weapon
        [SerializeField] public WeaponParams WeaponParamsLocal;
        [SerializeField] public WeaponParamsSO WeaponSo;
        [HideInInspector] public Transform OriginTransform;
        public float SignX;
        public float SignY;
        public Vector2 InitialVelocity;
        public IBlackBox Box;
        
        
        [HideInInspector] public Rigidbody2D Rigidbody;
        [HideInInspector] public SpriteRenderer SpriteRenderer;
        // StartPoint for RayCast 
        private Transform _tipTransform;
        public float BirthTime;
        
        [SerializeField] public ProjectileStateMachine StateMachine;
        
        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _tipTransform = transform.Find("Tip");
            BirthTime = Time.time;
            
            StateMachine = new ProjectileStateMachine(this);
        }

        private ISignalArray _inputArr;
        private ISignalArray _outputArr;
        private void Start() {
            _inputArr = Box.InputSignalArray;
            _outputArr = Box.OutputSignalArray;

            transform.localScale = WeaponParamsLocal.Size;
        
            StateMachine.Initialize(StateMachine.InitialFlight);

            DestroyItselfCoroutine = StartCoroutine(DestroyItself(WeaponParamsLocal.Lifespan));
            
            WeaponSo.DestroyProjectilesEvent += DestroyItselfNow;
        }

        private void OnDestroy() {
            WeaponSo.DestroyProjectilesEvent -= DestroyItselfNow;
        }

        private void DestroyItselfNow() {
            Destroy(OriginTransform.gameObject);
        }
        
        public Coroutine DestroyItselfCoroutine;
        public IEnumerator DestroyItself(float delay) {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
        
        public void ActivateBlackBoxCircle() {
            Box.ResetState();

            float maxDistance = WeaponParamsLocal.NNControlDistance * math.SQRT2;
            
            _inputArr[0] = Mathf.Lerp(-1f, 1f,Math.Abs(RelativePos.x) / WeaponParamsLocal.NNControlDistance);
            _inputArr[1] = Mathf.Lerp(-1f, 1f,Math.Abs(RelativePos.y) / WeaponParamsLocal.NNControlDistance);
            _inputArr[2] = Mathf.Lerp(-1f, 1f,DistanceFromOrigin / maxDistance);
            
            Box.Activate();
        }
        
        public void ActivateBlackBoxRect() {
            Box.ResetState();
            
            float maxX = WeaponParamsLocal.RectDimensions.x * WeaponParamsLocal.NNControlDistance;
            float maxY = WeaponParamsLocal.RectDimensions.y * WeaponParamsLocal.NNControlDistance;
            float maxDistance = maxX > maxY ? maxX : maxY;
            
            _inputArr[0] = Mathf.Lerp(-1f, 1f,Math.Abs(RelativePos.x) / maxX);
            _inputArr[1] = Mathf.Lerp(-1f, 1f,Math.Abs(RelativePos.y) / maxY);
            _inputArr[2] = Mathf.Lerp(-1f, 1f,DistanceFromOrigin / maxDistance);
            
            Box.Activate();
        }
        
        public void ActivateBlackBoxPolar() {
            Box.ResetState();
            
            float maxPhi = WeaponParamsLocal.MaxPolarAngleDeg * Mathf.Deg2Rad;
            float NNControlDistance = WeaponParamsLocal.NNControlDistance;
            
            float x = Math.Abs( DistanceFromOrigin * Mathf.Sin(PhiRad) );
            float y = Math.Abs( DistanceFromOrigin * Mathf.Cos(PhiRad) );
            
            float x_denominator = maxPhi >= 90f ? NNControlDistance : NNControlDistance * Mathf.Sin(maxPhi);
            
            _inputArr[0] = Mathf.Lerp(-1f, 1f,x / x_denominator);
            _inputArr[1] = Mathf.Lerp(-1f, 1f,y / NNControlDistance);
            _inputArr[2] = Mathf.Lerp(-1f, 1f,DistanceFromOrigin / NNControlDistance);
        
            Box.Activate();
        }
        
        private float _hue, _maxSpeed, _force;
        private Vector2 _vel;
        public void ReadDataFromBlackBox() {
            float x = Mathf.Lerp(-1f, 1f, (float)_outputArr[0]) * SignX;
            float y = Mathf.Lerp(-1f, 1f, (float)_outputArr[1]) * SignY;
        
            _vel = x * OriginTransform.right + y * OriginTransform.up;
        
            _hue = Mathf.Lerp(WeaponParamsLocal.HueRange.x, WeaponParamsLocal.HueRange.y, (float)_outputArr[2]);
            _maxSpeed = Mathf.Lerp(WeaponParamsLocal.SpeedRange.x, WeaponParamsLocal.SpeedRange.y, (float)_outputArr[3]);
            _force = Mathf.Lerp(WeaponParamsLocal.ForceRange.x, WeaponParamsLocal.ForceRange.y, (float)_outputArr[4]);
            
            SpriteRenderer.color = Color.HSVToRGB(_hue, WeaponParamsLocal.Saturation, WeaponParamsLocal.Brightness);
            Rigidbody.AddForce(_vel * _force);
            
            if (WeaponParamsLocal.ForwardForce)
                Rigidbody.AddForce(OriginTransform.up * _force);
        }

        public void LimitMaxSpeed() {
            float speed = Rigidbody.velocity.magnitude;
            if (speed > _maxSpeed)
                Rigidbody.velocity = Rigidbody.velocity.normalized * _maxSpeed;
        }

        private void Update() {
            CalcProjectileStats();
            StateMachine.Update();
        }
        
        private void FixedUpdate() {
            CheckCollision();
            StateMachine.FixedUpdate();
            transform.up = Rigidbody.velocity;
        }

        private void LateUpdate() {
            // Transition to PauseState is common to any state, so we check it here
            // Do not pause if it's already paused
            // Do not pause if it's on UI layer
            if (GlobalVariables.IsPaused && StateMachine.CurrentState != StateMachine.Pause && gameObject.layer != 5)
                StateMachine.TransitionTo(StateMachine.Pause);
            
            StateMachine.LateUpdate();
        }

        
        private void OnDrawGizmosSelected() {
            Gizmos.DrawRay(OriginTransform.position, RelativePosDir);
        }


        public Vector2 RelativePos;
        public Vector2 RelativePosDir;
        public float DistanceFromOrigin;
        public float PhiRad;
        private void CalcProjectileStats() {
            RelativePosDir = transform.position - OriginTransform.position;
            RelativePos = transform.localPosition;
            DistanceFromOrigin = RelativePos.magnitude;
            PhiRad = Vector2.Angle(OriginTransform.up, RelativePosDir) * Mathf.Deg2Rad;
        }

        
        [SerializeField] private int _layerMask = 0b_0000_0000_1000;
        [SerializeField] private float _damage = 1f;
        private void CheckCollision() {
            Vector2 startPoint = _tipTransform.position;
            RaycastHit2D hit = Physics2D.CircleCast(startPoint, transform.localScale.x, -transform.up, transform.localScale.y, _layerMask);
            
            if (ReferenceEquals(hit.collider, null))
                return;
        
            IDamagable objectToDamage = hit.transform.GetComponent<IDamagable>();
            GameManager.Instance.DamageObject(objectToDamage, _damage);
            //Debug.Log(objectToDamage);
        
            Destroy(gameObject);
        }


        [field: SerializeField] public float HealthPoints { get; set; } = 2f;
        public void TakeDamage(float damage) {
            HealthPoints -= damage;
            if (HealthPoints <= 0)
                Destroy(gameObject);
        }

    }
}
