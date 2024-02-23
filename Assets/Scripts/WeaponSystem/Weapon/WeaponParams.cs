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
        [field: SerializeField] [field: Range(0.3f, 1f)] public float FireRate { get; set; }
        [field: SerializeField] [field: Range(1, 20)] public int ProjectilesInOneShot { get; set; }
        
        [field: Header("Projectile controls")]
        [field: SerializeField] public Vector2 Size { get; set; }
        [field: SerializeField] [field: Range(2f, 10f)] public float Lifespan { get; set; }
        
        [field: SerializeField] [field: MinMaxRange(0f, 1f, 2)] public Vector2 HueRange { get; set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float Saturation { get; set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float Brightness { get; set; }
        
        [field: Header("NN Pattern controls")] 
        [field: SerializeField] [field: MinMaxRange(1f, 8f)] public Vector2 SpeedRange { get; set; }
        [field: SerializeField] [field: MinMaxRange(0.5f, 5f)] public Vector2 ForceRange { get; set; }
        [field: SerializeField] [field: Range(1f, 8f)] public float NNControlDistance { get; set; }
        [field: SerializeField] public bool FlipY { get; set; }
        [field: SerializeField] public bool ForwardForce { get; set; }
        
        [field:Header("Initial Flight controls")] 
        [field: SerializeField] [field:Range(0.05f, 0.4f)] public float InitialFlightRadius { get; set; }
        [field: SerializeField] [field:Range(0.5f, 5f)] public float InitialSpeed { get; set; }
        [field: SerializeField] [field:Range(1f, 179f)] public float Angle { get; set; }
        
        [field:Header("Reflection controls")] 
        [field: SerializeField] public bool FlipXOnReflect { get; set; }
        [field: SerializeField] public bool FlipYOnReflect { get; set; }
        [field: SerializeField] public ProjectileMode Mode { get; set; }
        
        [field: SerializeField] [field:Range(math.SQRT2, 2f)] public float ReflectiveCircleRadius { get; set; }
        [field: SerializeField] public Vector2 RectDimensions { get; set; }
        [field: SerializeField] [field: Range(5f, 179.9f)] public float MaxPolarAngleDeg { get; set; }

        
        public WeaponParams(IWeaponParams weaponParams) {
            Copy(weaponParams, this);
        }

        /// <summary>
        /// Dublicate data from original to copy.
        /// </summary>
        public static void Copy(IWeaponParams Original, IWeaponParams Copy) {
            Copy.FireRate = Original.FireRate;
            Copy.ProjectilesInOneShot = Original.ProjectilesInOneShot;
            
            Copy.Size = Original.Size;
            Copy.Lifespan = Original.Lifespan;
            Copy.HueRange = Original.HueRange;
            Copy.Saturation = Original.Saturation;
            Copy.Brightness = Original.Brightness;
            
            Copy.SpeedRange = Original.SpeedRange;
            Copy.ForceRange = Original.ForceRange;
            Copy.NNControlDistance = Original.NNControlDistance;
            Copy.FlipY = Original.FlipY;
            Copy.ForwardForce = Original.ForwardForce;
            
            Copy.InitialFlightRadius = Original.InitialFlightRadius;
            Copy.InitialSpeed = Original.InitialSpeed;
            Copy.Angle = Original.Angle;
            
            Copy.FlipXOnReflect = Original.FlipXOnReflect;
            Copy.FlipYOnReflect = Original.FlipYOnReflect;
            Copy.Mode = Original.Mode;
            
            Copy.ReflectiveCircleRadius = Original.ReflectiveCircleRadius;
            Copy.RectDimensions = Original.RectDimensions;
            Copy.MaxPolarAngleDeg = Original.MaxPolarAngleDeg;
        }
        
        
        public WeaponParams() { }
    }
}
