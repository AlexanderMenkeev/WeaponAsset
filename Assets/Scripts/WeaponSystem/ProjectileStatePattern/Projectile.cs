using System;
using SharpNeat.Phenomes;
using UnityEngine;
using UnityEngine.Serialization;

namespace WeaponSystem.ProjectileStatePattern {
    public class Projectile : MonoBehaviour
    {
        public Rigidbody2D Rigidbody;
        public SpriteRenderer _spriteRenderer;
        public Transform ParentTransform;
        public Transform _pivotPoint;
        public Camera _camera;

        [SerializeField] private GameObject _temporalObjects;
    
        [SerializeField] public CommonVariablesSO _commonVariables;
    
        protected float _birthTime;
        public float Lifespan;
    
        public IBlackBox Box;
        protected ISignalArray _inputArr;
        protected ISignalArray _outputArr;
    
        public float MinSpeed;
        public float MaxSpeed;
        public float MinForce;
        public float MaxForce;
        [FormerlySerializedAs("MaxDistance")] public float NeuralNetworkControlDistance;
        public float SignX;
        public float SignY;
        public Vector2 InitialVelocity;

        public bool IsUI;
    
        private GameObject _emptyGameObject;

    
        [SerializeField] public ProjectileStateMachine StateMachine;
    

        private void Awake()
        {
            _camera = Camera.main;
            Rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _pivotPoint = transform.Find("PivotPoint");
            Damage = 5f;
            IsUI = false;
        
            StateMachine = new ProjectileStateMachine(this);
        }

        private void Start()
        {
            _inputArr = Box.InputSignalArray;
            _outputArr = Box.OutputSignalArray;
        
            StateMachine.Initialize(StateMachine.StraightFlight);
        
            _birthTime = Time.time;
            _currPos = Rigidbody.position;
            _commonVariables.OnPauseResumeEvent += OnPauseResumeGame;
        }
    
        private void OnDestroy() {
            _commonVariables.OnPauseResumeEvent -= OnPauseResumeGame;
        }
    
        protected void OnPauseResumeGame() {
            Rigidbody.velocity = Vector2.zero;
        
        }

        protected void CheckCollision()
        {
            int layerMask = 0b_0000_0000_1000;
            Vector2 startPoint = _pivotPoint.position;
            RaycastHit2D hit = Physics2D.CircleCast(startPoint, transform.localScale.x, -transform.up, transform.localScale.y, layerMask);
        
        
            if (ReferenceEquals(hit.collider, null))
                return;
        
            IDamagable objectToDamage = hit.transform.GetComponent<IDamagable>();
            LocalGameManager.Instance.DamageObject(objectToDamage, Damage);
            Debug.Log(objectToDamage);
        
            Destroy(gameObject);
        }


        protected float Damage;

        public void DestroyYourself()
        {
            if (Time.time - _birthTime > Lifespan) 
                Destroy(ParentTransform.gameObject);
        
        }
    

    
        public float ReflectiveCircleRadius;
        public void ActivateBlackBox()
        {
            Box.ResetState();
        
            _inputArr[0] = Mathf.Lerp(-1f, 1f,Math.Abs(RelativePos.x) / NeuralNetworkControlDistance);
            _inputArr[1] = Mathf.Lerp(-1f, 1f,Math.Abs(RelativePos.y) / NeuralNetworkControlDistance);
            _inputArr[2] = Mathf.Lerp(-1f, 1f,DistanceFromOrigin / NeuralNetworkControlDistance);
        
            Box.Activate();
        }
    
    
        private float _hue, _saturation, _brightness, _maxSpeed, _force;
        private Vector2 _vel;
        public void ReadDataFromBlackBox() {
            float x = Mathf.Lerp(-1f, 1f, (float)_outputArr[0]) * SignX;
            float y = Mathf.Lerp(-1f, 1f, (float)_outputArr[1]) * SignY;
        
            _vel = x * ParentTransform.right + y * ParentTransform.up;
        
            _hue = Mathf.Lerp(0.1f, 1f, (float)_outputArr[2]);
            _maxSpeed = Mathf.Lerp(MinSpeed, MaxSpeed, (float)_outputArr[3]);
            _force = Mathf.Lerp(MinForce, MaxForce, (float)_outputArr[4]);
            
        
            
        
            _saturation = 0.95f;
            _brightness = 0.95f;
            _spriteRenderer.color = Color.HSVToRGB(_hue, _saturation, _brightness);
        
        
            Rigidbody.AddForce(_vel * _force);
        }

        public void LimitMaxSpeed() {
            float speed = Rigidbody.velocity.magnitude;
            if (speed > _maxSpeed)
                Rigidbody.velocity = Rigidbody.velocity.normalized * _maxSpeed;
        }

        private void Update() {
            StateMachine.Update();
        }

        private Vector2 _prevPos;
        private Vector2 _currPos;
        public Vector2 ActualVelocity;
        private void FixedUpdate() {
            CalcProjectileStats();
            StateMachine.FixedUpdate();
            transform.up = Rigidbody.velocity;
            CheckCollision();
        }

        private void LateUpdate() {
            StateMachine.LateUpdate();
        }
    
    
        public Vector2 RelativePos;
        public float DistanceFromOrigin;
        protected void CalcProjectileStats() {
            RelativePos = ParentTransform.InverseTransformPoint(transform.position);
            DistanceFromOrigin = RelativePos.magnitude;
        
            _prevPos = _currPos;
            _currPos = Rigidbody.position;
            ActualVelocity = (_currPos - _prevPos) / Time.fixedDeltaTime;
        }
    
    }
}
