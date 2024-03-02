using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.DemoScene {
    public class DemoWeapon : AbstractWeapon {
        
        public void UpdateWeaponSO(WeaponParamsSO weaponSo) {
            if (FireCoroutine != null) 
                StopCoroutine(FireCoroutine);
            
            _weaponSO.DestroyProjectilesEvent?.Invoke();
            _weaponSO = weaponSo;
            base.InitializeParams();

            FireCoroutine = StartCoroutine(Fire());
        }
        
        
    }
}
