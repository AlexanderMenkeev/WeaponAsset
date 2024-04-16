using System;
using System.Collections.Generic;
using System.Xml;
using NeatBullets.Core.Scripts.Interfaces;
using NeatBullets.Core.Scripts.WeaponSystem;
using NeatBullets.Core.Scripts.WeaponSystem.NEAT;
using SharpNeat.Genomes.Neat;
using UnityEngine;

namespace NeatBullets.ShadowSurvival.Scripts.Player {
    public class GameWeapon : AbstractWeapon
    {
        private void Awake() {
            base.OnAwakeFunc();
            _weaponSO.ResetFunc();
            _weaponSO.Size = new Vector2(0.03f, 0.18f);
            _weaponSO.Brightness = 1f;
            _weaponSO.Saturation = 1f;
        }
    
        private void OnDestroy() {
            base.OnDestroyFunc();
        }

        private void Start() {
            base.InitializeParams();
        }

        public void UpdateWeaponSO(IWeaponParams newParams) {
            WeaponParams.Copy(newParams, _weaponSO);
            _weaponParamsLocal = new WeaponParams(_weaponSO);
        }
    }
}