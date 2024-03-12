using NeatProjectiles.Core.Scripts.WeaponSystem;
using UnityEngine;

namespace NeatProjectiles.GameExample.Scripts.UIScripts {
    public class CanvasWeapon : AbstractWeapon
    {
        private void Awake() {
            ProjectilesParentTransform = GameObject.Find("TemporalObjects").transform;
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
