using NEATProjectiles.Core.Scripts.SODefinitions;
using NEATProjectiles.Core.Scripts.WeaponSystem;

namespace NEATProjectiles.Demos.Scripts.UIScripts {
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
