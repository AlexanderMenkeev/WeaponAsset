using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts {
    
    // Area is needed for mapReposition
    public class Area : MonoBehaviour {
        private Player _player;
        private void Awake() {
            _player = FindObjectOfType<Player>();
        }

        // Follow player object without rotating with it
        private void FixedUpdate() {
            transform.position = _player.transform.position;
        }
    }
}
