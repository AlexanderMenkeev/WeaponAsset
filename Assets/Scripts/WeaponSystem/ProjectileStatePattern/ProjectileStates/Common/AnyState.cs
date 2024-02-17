using Interfaces;
using UnityEngine;

namespace WeaponSystem.ProjectileStatePattern.ProjectileStates.Common {
    
public class AnyState : IState {
    private Projectile _projectile;
    
    public AnyState(Projectile projectile) {
        _projectile = projectile;
    }


    private Vector2 _prevVelocity;
    public void Enter() { }

    public void Update() { }

    public void FixedUpdate() { }

    
    public void LateUpdate() { }

    public void Exit() { }
}

}