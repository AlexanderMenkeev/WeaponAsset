using NEATProjectiles.Core.Scripts.WeaponSystem;
using UnityEngine;

namespace NEATProjectiles.Demos.Scripts.Player {
    public class GameWeapon : AbstractWeapon
    {
        private void Awake() {
            TemporalObjects = GameObject.Find("TemporalObjects");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
           
            _weaponSO.UpdateParamsEvent += InitializeParams;
        }
        
        private void OnDestroy() {
            _weaponSO.UpdateParamsEvent -= InitializeParams;
        }
        
        private void Start() {
            InitializeParams();
        }
        
    }
}
