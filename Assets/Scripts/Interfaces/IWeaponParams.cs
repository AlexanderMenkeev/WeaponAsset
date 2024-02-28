using SODefinitions;
using UnityEngine;

namespace Interfaces {
    public interface IWeaponParams {
        
        float FireRate { get; set; }
        int ProjectilesInOneShot { get; set; }
        
        Vector2 Size { get; set; }
        float Lifespan { get; set; }
        Vector2 HueRange { get; set; }
        float Saturation { get; set; }
        float Brightness { get; set; }
        
        Vector2 SpeedRange { get; set; }
        Vector2 ForceRange { get; set; }
        float NNControlDistance { get; set; }
        bool FlipY { get; set; }
        bool ForwardForce { get; set; }
        
        float InitialFlightRadius { get; set; }
        float InitialSpeed { get; set; }
        float Angle { get; set; }
        
        bool FlipXOnReflect { get; set; }
        bool FlipYOnReflect { get; set; }
        ProjectileMode Mode { get; set; }
        
        float ReflectiveCircleRadius { get; set; }
        Vector2 RectDimensions { get; set; }
        float MaxPolarAngleDeg { get; set; }
    }
}