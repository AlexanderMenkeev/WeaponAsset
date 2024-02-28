using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts {
    public class MapReposition : MonoBehaviour
    {
        
        private GameObject _player;
        private void Awake() {
            _player = GameObject.Find("Player");
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Area"))
                return;

            Vector3 playerPos = _player.transform.position;
            Vector3 myPos = transform.position;

            float dx = Mathf.Abs(playerPos.x - myPos.x);
            float dy = Mathf.Abs(playerPos.y - myPos.y);

            Vector2 playerDir = _player.GetComponent<Player>()._rigidbody.velocity;
        
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;

            if (dx > dy)
                transform.Translate(Vector3.right * dirX * 64);
            else if (dx < dy)
                transform.Translate(Vector3.up * dirY * 64);
        
        }

    }
}
