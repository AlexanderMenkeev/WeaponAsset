using Unity.Mathematics;
using UnityEngine;

namespace Interfaces {
    public interface IWeaponParams {
        float FireRate { get; set; }
        int ProjectilesInOneShot { get; set; }
        
        Vector2 Size { get; set; }
        float Lifespan { get; set; }
        
        float MinSpeed { get; set; }
        float MaxSpeed { get; set; }
        float MinForce { get; set; }
        float MaxForce { get; set; }
        float NNControlDistance { get; set; }
        float MaxPolarAngle { get; set; }
        bool FlipY { get; set; }

        float ReflectiveCircleRadius { get; set; }
        
        float InitialFlightRadius { get; set; }
        float InitialSpeed { get; set; }
        int Angle { get; set; }
    }
}
