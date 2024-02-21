using System;
using Editor.MinMaxRangeAttribute;
using Interfaces;
using SODefinitions;
using Unity.Mathematics;
using UnityEngine;

namespace WeaponSystem.Weapon {
    [Serializable]
    public class WeaponParams : IWeaponParams {
    
        [field: Header("Weapon controls")] 
        [field: SerializeField] [field: Range(0.2f, 1f)] public float FireRate { get; set; }
        [field: SerializeField] [field: Range(1, 20)] public int ProjectilesInOneShot { get; set; }
        
        [field: Header("Projectile controls")]
        [field: SerializeField] public Vector2 Size { get; set; }
        [field: SerializeField] [field: MinMaxRange(0f, 1f)] public Vector2 HueRange { get; set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float Saturation { get; set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float Brightness { get; set; }
        [field: SerializeField] [field: Range(2f, 10f)] public float Lifespan { get; set; }

        [field: Header("NN Pattern controls")] 
        [field: SerializeField] [field: MinMaxRange(1f, 8f)] public Vector2 SpeedRange { get; set; }
        [field: SerializeField] [field: MinMaxRange(0.5f, 5f)] public Vector2 ForceRange { get; set; }
        [field: SerializeField] [field: Range(1f, 12f)] public float NNControlDistance { get; set; }
        [field: SerializeField] [field: Range(5f, 180f)] public float MaxPolarAngleDeg { get; set; }
        [field: SerializeField] public bool FlipY { get; set; }
        
        [field:Header("Reflection controls")] 
        [field: SerializeField] [field:Range(math.SQRT2, 2f)] public float ReflectiveCircleRadius { get; set; }
        
        [field:Header("Initial Flight controls")] 
        [field: SerializeField] [field:Range(0.05f, 0.5f)] public float InitialFlightRadius { get; set; }
        [field: SerializeField] [field:Range(0.5f, 5f)] public float InitialSpeed { get; set; }
        [field: SerializeField] [field:Range(4, 179)] public int Angle { get; set; }
        
        [field: SerializeField] public Vector2 RectDimensions { get; set; }
        [field: SerializeField] public bool FlipXOnReflect { get; set; }
        [field: SerializeField] public bool FlipYOnReflect { get; set; }
        [field: SerializeField] public bool ForwardForce { get; set; }
        
        [field: SerializeField] public ProjectileMode Mode { get; set; }



        public WeaponParams(IWeaponParams weaponParams) {
            FireRate = weaponParams.FireRate;
            ProjectilesInOneShot = weaponParams.ProjectilesInOneShot;
            
            Size = weaponParams.Size;
            HueRange = weaponParams.HueRange;
            Saturation = weaponParams.Saturation;
            Brightness = weaponParams.Brightness;
            Lifespan = weaponParams.Lifespan;
        
            SpeedRange = weaponParams.SpeedRange;
            ForceRange = weaponParams.ForceRange;
            NNControlDistance = weaponParams.NNControlDistance;
            MaxPolarAngleDeg = weaponParams.MaxPolarAngleDeg;
            
            FlipY = weaponParams.FlipY;
            
            ReflectiveCircleRadius = weaponParams.ReflectiveCircleRadius;
            
            InitialFlightRadius = weaponParams.InitialFlightRadius;
            InitialSpeed = weaponParams.InitialSpeed;
            Angle = weaponParams.Angle;

            RectDimensions = weaponParams.RectDimensions;
            FlipXOnReflect = weaponParams.FlipXOnReflect;
            FlipYOnReflect = weaponParams.FlipYOnReflect;
            ForwardForce = weaponParams.ForwardForce;
            
            Mode = weaponParams.Mode;
        }
        
        public WeaponParams() {
        
        }
    }
}
