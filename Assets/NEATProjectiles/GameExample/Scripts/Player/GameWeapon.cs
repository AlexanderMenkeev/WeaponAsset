using NeatProjectiles.Core.Scripts.WeaponSystem;
using UnityEngine;

namespace NeatProjectiles.GameExample.Scripts.Player {
    public class GameWeapon : AbstractWeapon
    {
        private Player _player;
        [SerializeField] public GameObject TurretBase;
        private void Awake() {
            base.OnAwakeFunc();
            _player = FindObjectOfType<Player>();
            TurretBase = GameObject.Find("TurretBase");
        }
        
        private void OnDestroy() {
            base.OnDestroyFunc();
        }
        
        private void Start() {
            InitializeParams();
            _weaponParamsLocal.InitialSpeed = 4f;
        }
        
        // Follow player object without rotating with it
        private void LateUpdate() {
            TurretBase.transform.position = _player.transform.position;
        }
        
        
        
    }
}
