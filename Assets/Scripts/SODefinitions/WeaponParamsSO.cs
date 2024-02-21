using Editor.MinMaxRangeAttribute;
using Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace SODefinitions {
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponParamsSO")]
    public class WeaponParamsSO : ScriptableObject, IWeaponParams
    {
        [field: SerializeField] [field: Range(0.2f, 1f)] public float FireRate { get; set; }
        [field: SerializeField] [field: Range(1, 20)] public int ProjectilesInOneShot { get; set; }
        
        
        [field: SerializeField] public Vector2 Size { get; set; }
        [field: SerializeField] [field: Range(2f, 10f)] public float Lifespan { get; set; }
        
        [field: SerializeField] [field: MinMaxRange(0f, 1f, 2)] public Vector2 HueRange { get; set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float Saturation { get; set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float Brightness { get; set; }
        
        
        
        [field: SerializeField] [field: MinMaxRange(1f, 8f)] public Vector2 SpeedRange { get; set; }
        [field: SerializeField] [field: MinMaxRange(0.5f, 5f)] public Vector2 ForceRange { get; set; }
        
        [field: SerializeField] [field: Range(1f, 7f)] public float NNControlDistance { get; set; }
        [field: SerializeField] [field: Range(10f, 180f)] public float MaxPolarAngleDeg { get; set; }
        [field: SerializeField] public bool FlipY { get; set; }
        
        
        [field: SerializeField] [field:Range(math.SQRT2, 2f)] public float ReflectiveCircleRadius { get; set; }
        
        
        [field: SerializeField] [field:Range(0.05f, 0.4f)] public float InitialFlightRadius { get; set; }
        [field: SerializeField] [field:Range(0.5f, 3f)] public float InitialSpeed { get; set; }
        [field: SerializeField] [field:Range(10, 180)] public int Angle { get; set; }
        
        [field: SerializeField] public Vector2 RectDimensions { get; set; }
        
        [field: SerializeField] public bool FlipXOnReflect { get; set; }
        [field: SerializeField] public bool FlipYOnReflect { get; set; }
        [field: SerializeField] public bool ForwardForce { get; set; }

        [field: SerializeField] public ProjectileMode Mode { get; set; }

        //[SerializeField] public TextAsset asset;
        
        public delegate void UpdateDelegate();
        public UpdateDelegate UpdateParamsEvent;
    
        private void InitializeParams() {
            FireRate = 1f;
            ProjectilesInOneShot = 10;
            
            
            Size = new Vector2(0.03f, 0.18f);
            HueRange = new Vector2(0f, 1f);
            Saturation = 0.9f;
            Brightness = 0.9f;
            
            Lifespan = 6f;

            SpeedRange = new Vector2(3f, 6f);
            ForceRange = new Vector2(1f, 3f);
            
            NNControlDistance = 3f;
            MaxPolarAngleDeg = 15f;
            FlipY = true;
            
            ReflectiveCircleRadius = 1.5f;

            InitialFlightRadius = 0.1f;
            InitialSpeed = 1f;
            Angle = 45;

            RectDimensions = new Vector2(1f, 2f);
            FlipXOnReflect = true;
            FlipYOnReflect = true;
            ForwardForce = false;

            Mode = ProjectileMode.CircleReflection;
        }

        public void OnEnable() {
            InitializeParams();
        }

        private void OnValidate() {
            UpdateParamsEvent?.Invoke();
        }

        
    }
    
    
    public enum ProjectileMode {
        CircleReflection,
        RectangleReflection,
        Polar
    }
}
