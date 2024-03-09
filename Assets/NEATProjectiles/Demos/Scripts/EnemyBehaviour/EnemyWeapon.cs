using NEATProjectiles.Core.Scripts.SODefinitions;
using NEATProjectiles.Core.Scripts.WeaponSystem;
using UnityEngine;

namespace NEATProjectiles.Demos.Scripts.EnemyBehaviour {
    public class EnemyWeapon : AbstractWeapon
    {
        private void Awake() {
            TemporalObjects = GameObject.Find("TemporalObjects");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
        }
        private void Start() {
            base.InitializeParams();
        }
        
        public void UpdateWeaponSO(WeaponParamsSO weaponSo) {
            _weaponSO = weaponSo;
            base.InitializeParams();
        }

    }
}
