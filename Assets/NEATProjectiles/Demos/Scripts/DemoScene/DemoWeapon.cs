using NEATProjectiles.Core.Scripts.SODefinitions;
using NEATProjectiles.Core.Scripts.WeaponSystem;

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
        
        
    }
}
