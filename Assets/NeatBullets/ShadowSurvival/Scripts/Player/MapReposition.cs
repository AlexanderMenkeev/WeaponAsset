using UnityEngine;

namespace NeatBullets.ShadowSurvival.Scripts.Player {
    public class MapReposition : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Transform _pivot;
        private void Awake() {
            _player = FindObjectOfType<Player>();
            _pivot = transform.Find("Pivot");
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            
            if (!other.CompareTag("Area") || _player == null)
                return;
            
            Vector3 playerPos = _player.transform.position;
            Vector3 pivotPos = _pivot.position;

            float dx = Mathf.Abs(playerPos.x - pivotPos.x);
            float dy = Mathf.Abs(playerPos.y - pivotPos.y);

            Vector2 playerDir = _player.GetComponent<Player>().ActualVelocity;
        
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;

            if (dx > dy)
                transform.Translate(Vector3.right * dirX * 80);
            else if (dx < dy)
                transform.Translate(Vector3.up * dirY * 80);
        
        }

    }
}
