using Unity.Mathematics;
using UnityEngine;

namespace SODefinitions {
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponSO")]
    public class WeaponParamsSO : ScriptableObject 
    {
        [Header("Projectile controls")]
        public Vector2 Size;
        [Range(2f, 10f)] public float Lifespan;

        [Header("Pattern controls")] 
        [Range(1f, 5f)] public float MinSpeed;
        [Range(5f, 10f)] public float MaxSpeed;
        [Range(1f, 2f)] public float MinForce;
        [Range(2f, 8f)] public float MaxForce;
        [Range(1f, 7f)] public float NNControlDistance;
        [Range(0.2f, 1f)] public float FireRate;
        [Range(1, 20)] public int ProjectilesInOneShot;
        public bool FlipY;
        [Range(math.SQRT2, 2f)] public float ReflectiveCircleRadius;
    
        public delegate void UpdateDelegate();
        public UpdateDelegate UpdateParamsEvent;
    
        private void InitializeParams() {
            Size = new Vector2(0.03f, 0.18f);
            Lifespan = 5f;
            MinSpeed = 3f;
            MaxSpeed = 6f;
            MinForce = 1f;
            MaxForce = 4f;
            NNControlDistance = 3f;
            FireRate = 1f;
            ProjectilesInOneShot = 10;
            FlipY = true;
            ReflectiveCircleRadius = 1.5f;
        }

        public void OnEnable() {
            InitializeParams();
        }

        private void OnValidate() {
            UpdateParamsEvent?.Invoke();
        }
    
        
    }
}
