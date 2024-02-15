using System;
using SODefinitions;
using Unity.Mathematics;
using UnityEngine;

namespace WeaponSystem.ProjectileStatePattern {
    [Serializable]
    public class WeaponParams {
    
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

        public WeaponParams(WeaponParamsSO weaponParamsSO) {
            Size = weaponParamsSO.Size;
            Lifespan = weaponParamsSO.Lifespan;
        
            MinSpeed = weaponParamsSO.MinSpeed;
            MaxSpeed = weaponParamsSO.MaxSpeed;
            MinForce = weaponParamsSO.MinForce;
            MaxForce = weaponParamsSO.MaxForce;
            NNControlDistance = weaponParamsSO.NNControlDistance;
            FireRate = weaponParamsSO.FireRate;
            ProjectilesInOneShot = weaponParamsSO.ProjectilesInOneShot;
            FlipY = weaponParamsSO.FlipY;
            ReflectiveCircleRadius = weaponParamsSO.ReflectiveCircleRadius;
        }
        
        // copy constructor
        public WeaponParams(WeaponParams weaponParams) {
            Size = weaponParams.Size;
            Lifespan = weaponParams.Lifespan;
        
            MinSpeed = weaponParams.MinSpeed;
            MaxSpeed = weaponParams.MaxSpeed;
            MinForce = weaponParams.MinForce;
            MaxForce = weaponParams.MaxForce;
            NNControlDistance = weaponParams.NNControlDistance;
            FireRate = weaponParams.FireRate;
            ProjectilesInOneShot = weaponParams.ProjectilesInOneShot;
            FlipY = weaponParams.FlipY;
            ReflectiveCircleRadius = weaponParams.ReflectiveCircleRadius;
        }
        
        public WeaponParams() {
        
        }
    }
}
