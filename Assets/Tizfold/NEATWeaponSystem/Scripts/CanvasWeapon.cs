using System.Collections;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts {
    public class CanvasWeapon : AbstractWeapon
    {
        private void Awake() {
            TemporalObjects = GameObject.Find("TemporalObjects");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
            _weaponSO.UpdateParamsEvent += InitializeParams;
        }
        
        private void OnDestroy() {
            _weaponSO.UpdateParamsEvent -= InitializeParams;
        }

        protected override void InitializeParams() {
            _weaponParamsLocal = new WeaponParams(_weaponSO);
        }

        private void Start() {
            InitializeParams();
        }
        
    }
}
