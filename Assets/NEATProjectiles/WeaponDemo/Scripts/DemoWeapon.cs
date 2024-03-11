using System.Buffers.Text;
using Cinemachine.Editor;
using NEATProjectiles.Core.Scripts.SODefinitions;
using NEATProjectiles.Core.Scripts.WeaponSystem;
using UnityEngine;

namespace NEATProjectiles.Demos.Scripts.DemoScene {
    public class DemoWeapon : AbstractWeapon {
        
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
