using System;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour {
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
