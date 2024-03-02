using System.Collections;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts {
    public class GameWeapon : AbstractWeapon
    {
        private void Awake() {
            TemporalObjects = GameObject.Find("TemporalObjects");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
        }
        
        private void Start() {
            InitializeParams();
        }
        
    }
}
