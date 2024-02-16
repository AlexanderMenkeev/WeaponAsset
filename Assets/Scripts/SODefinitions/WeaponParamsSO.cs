using Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace SODefinitions {
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponParamsSO")]
    public class WeaponParamsSO : ScriptableObject, IWeaponParams
    {
        [field: Header("Weapon controls")] 
        [field: SerializeField] [field: Range(0.2f, 1f)] public float FireRate { get; set; }
        [field: SerializeField] [field: Range(1, 20)] public int ProjectilesInOneShot { get; set; }
        
        [field: Header("Projectile controls")]
        [field: SerializeField] public Vector2 Size { get; set; }
        [field: SerializeField] [field: Range(2f, 10f)] public float Lifespan { get; set; }

        [field: Header("NN Pattern controls")] 
        [field: SerializeField] [field: Range(1f, 5f)] public float MinSpeed { get; set; }
        [field: SerializeField] [field: Range(5f, 8f)] public float MaxSpeed { get; set; }
        [field: SerializeField] [field: Range(0.5f, 2f)] public float MinForce { get; set; }
        [field: SerializeField] [field: Range(2f, 5f)] public float MaxForce { get; set; }
        [field: SerializeField] [field: Range(1f, 7f)] public float NNControlDistance { get; set; }
        [field: SerializeField] public bool FlipY { get; set; }
        
        [field:Header("Reflection controls")] 
        [field: SerializeField] [field:Range(math.SQRT2, 2f)] public float ReflectiveCircleRadius { get; set; }
        
        [field:Header("Initial Flight controls")] 
        [field: SerializeField] [field:Range(0.05f, 0.4f)] public float InitialFlightRadius { get; set; }
        [field: SerializeField] [field:Range(0.5f, 3f)] public float InitialSpeed { get; set; }
        [field: SerializeField] [field:Range(10, 180)] public int Angle { get; set; }
        
        
        public delegate void UpdateDelegate();
        public UpdateDelegate UpdateParamsEvent;
    
        private void InitializeParams() {
            FireRate = 1f;
            ProjectilesInOneShot = 10;
            
            Size = new Vector2(0.03f, 0.18f);
            Lifespan = 6f;
            
            MinSpeed = 3f;
            MaxSpeed = 6f;
            MinForce = 1f;
            MaxForce = 3f;
            NNControlDistance = 3f;
            FlipY = true;
            
            ReflectiveCircleRadius = 1.5f;

            InitialFlightRadius = 0.1f;
            InitialSpeed = 1f;
            Angle = 45;
        }

        public void OnEnable() {
            InitializeParams();
        }

        private void OnValidate() {
            UpdateParamsEvent?.Invoke();
        }

        
    }
}
