using System;
using SharpNeat.Phenomes;
using SODefinitions;
using Unity.Mathematics;
using UnityEngine;
using WeaponSystem.Weapon;

namespace WeaponSystem.ProjectileStatePattern {
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public Rigidbody2D Rigidbody;
        [HideInInspector] public SpriteRenderer SpriteRenderer;
        private Transform _tipTransform;
        
        // assigned from the editor
        [SerializeField] public GlobalVariablesSO GlobalVariables;
        
        // assigned from the AbstractWeapon
        [SerializeField] public WeaponParams WeaponParamsLocal;
        [HideInInspector] public Transform OriginTransform;
        public float SignX;
        public float SignY;
        public Vector2 InitialVelocity;
        public IBlackBox Box;
        
        private ISignalArray _inputArr;
        private ISignalArray _outputArr;
        private float _birthTime;
        [SerializeField] public ProjectileStateMachine StateMachine;
        
        private void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _tipTransform = transform.Find("Tip");
            
            StateMachine = new ProjectileStateMachine(this);
        }

        private void Start() {
            _inputArr = Box.InputSignalArray;
            _outputArr = Box.OutputSignalArray;

            transform.localScale = WeaponParamsLocal.Size;
        
            StateMachine.Initialize(StateMachine.InitialFlight);
        
            _birthTime = Time.time;
            _currPos = Rigidbody.position;
        }
        
        public void DestroyYourself() {
            if (Time.time - _birthTime > WeaponParamsLocal.Lifespan) 
                Destroy(OriginTransform.gameObject);
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
        
        private float _hue, _saturation, _brightness, _maxSpeed, _force;
        private Vector2 _vel;
        public void ReadDataFromBlackBox() {
            float x = Mathf.Lerp(-1f, 1f, (float)_outputArr[0]) * SignX;
            float y = Mathf.Lerp(-1f, 1f, (float)_outputArr[1]) * SignY;
        
            _vel = x * OriginTransform.right + y * OriginTransform.up;
        
            _hue = Mathf.Lerp(0.1f, 1f, (float)_outputArr[2]);
            
            _maxSpeed = Mathf.Lerp(WeaponParamsLocal.MinSpeed, WeaponParamsLocal.MaxSpeed, (float)_outputArr[3]);
            _force = Mathf.Lerp(WeaponParamsLocal.MinForce, WeaponParamsLocal.MaxForce, (float)_outputArr[4]);
            
            _saturation = 0.95f;
            _brightness = 0.95f;
            SpriteRenderer.color = Color.HSVToRGB(_hue, _saturation, _brightness);
            
            Rigidbody.AddForce(_vel * _force);
            
            
            if (WeaponParamsLocal.ForwardForce)
                Rigidbody.AddForce(OriginTransform.up * _force);
        }

        public void LimitMaxSpeed() {
            float speed = Rigidbody.velocity.magnitude;
            if (speed > _maxSpeed)
                Rigidbody.velocity = Rigidbody.velocity.normalized * _maxSpeed;
            
            // if (speed < WeaponParamsLocal.MinSpeed)
            //     Rigidbody.velocity = Rigidbody.velocity.normalized * WeaponParamsLocal.MinSpeed;
        }

        private void Update() {
            DestroyYourself();
            StateMachine.Update();
        }
        
        private void FixedUpdate() {
            
            CalcProjectileStats();
            StateMachine.FixedUpdate();
            transform.up = Rigidbody.velocity;
        }

        private void LateUpdate() {
            // Transition to PauseState is common to any state, so we check it here
            if (GlobalVariables.IsPaused)
                StateMachine.TransitionTo(StateMachine.Pause);
            
            StateMachine.LateUpdate();
        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawRay(OriginTransform.position, RelativePosDir);
            Gizmos.DrawRay(OriginTransform.position, OriginTransform.up);
        }


        public Vector2 RelativePos;
        public Vector2 RelativePosDir;
        public float DistanceFromOrigin;
        public float PhiRad;
        
        private Vector2 _prevPos;
        private Vector2 _currPos;
        public Vector2 ActualVelocity;
        private void CalcProjectileStats() {
            RelativePosDir = transform.position - OriginTransform.position;
            RelativePos = transform.localPosition;
            DistanceFromOrigin = RelativePos.magnitude;
            PhiRad = Vector2.Angle(OriginTransform.up, RelativePosDir) * Mathf.Deg2Rad;
            
            _prevPos = _currPos;
            _currPos = Rigidbody.position;
            ActualVelocity = (_currPos - _prevPos) / Time.fixedDeltaTime;
        }
    
    }
}
