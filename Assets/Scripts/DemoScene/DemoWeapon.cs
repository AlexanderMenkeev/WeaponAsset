using System;
using SODefinitions;
using UnityEngine;
using WeaponSystem.Weapon;

namespace DemoScene {
    public class DemoWeapon : AbstractWeapon {

        private Rigidbody2D _rigidbody2D;

        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void UpdateWeaponSO(WeaponParamsSO weaponSo) {
            if (FireCoroutine != null) 
                StopCoroutine(FireCoroutine);
            
            _weaponSO.DestroyProjectilesEvent?.Invoke();
            _weaponSO = weaponSo;
            base.InitializeParams();

            FireCoroutine = StartCoroutine(FireProjectile());
        }
        
        
    }
}
