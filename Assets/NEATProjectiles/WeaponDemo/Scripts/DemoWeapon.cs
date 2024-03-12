using System;
using NeatProjectiles.Core.Scripts.SODefinitions;
using NeatProjectiles.Core.Scripts.WeaponSystem;
using UnityEngine;

namespace NeatProjectiles.WeaponDemo.Scripts {
    public class DemoWeapon : AbstractWeapon {

        private void Awake() {
            if (ProjectilesParentTransform == null)
                ProjectilesParentTransform = GameObject.Find("TemporalObjects").transform;
            
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
        }
        
        public void UpdateWeaponSO(WeaponParamsSO weaponSo) {
            
            if (FireCoroutine != null) 
                StopCoroutine(FireCoroutine);
            
            if (_weaponSO != null)
                _weaponSO.DestroyProjectilesEvent?.Invoke();
            
            _weaponSO = weaponSo;
            base.InitializeParams();

            FireCoroutine = StartCoroutine(Fire());
        }

        public void LaunchForward() {
            foreach (CoordinateSystem system in CoordinateSystems) {
                system.IsMoving = true;
                system.MoveSpeed = 7f;
                system.Direction = Vector3.right;
                system.IsRotating = false;
            }
        }
        
        
    }
}
