using System;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour {
    public class EnemyWeapon : AbstractWeapon
    {
        public EnemyController Enemy;
        private void Awake() {
            Enemy = GetComponentInParent<EnemyController>();
            TemporalObjects = GameObject.Find("TemporalObjects");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
        }
        private void Start() {
            base.InitializeParams();
        }

    }
}
