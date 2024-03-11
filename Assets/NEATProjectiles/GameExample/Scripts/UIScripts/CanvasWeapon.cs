using NEATProjectiles.Core.Scripts.WeaponSystem;
using UnityEngine;

namespace NEATProjectiles.Demos.Scripts.UIScripts {
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
