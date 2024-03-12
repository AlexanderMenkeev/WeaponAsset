using UnityEngine;

namespace NeatProjectiles.Core.Scripts.WeaponSystem {
    
    public class CoordinateSystem : MonoBehaviour {
        
        public AbstractWeapon Weapon;
        
        private Transform _transform;
        private void Awake() {
            _transform = transform;
            IsMoving = false;
            IsRotating = false;
        }

        private void OnDestroy() {
            Weapon.CoordinateSystems.Remove(this);
        }

        // if all child projectiles were destroyed => destroy this CoordinateSystem
        private void LateUpdate() {
            if (transform.childCount == 0)
                Destroy(gameObject);
            
            Move();
            Rotate();
        }

        public bool IsMoving;
        public Vector3 Direction;
        public float MoveSpeed;
        private void Move() {
            if (IsMoving)
                _transform.position += Direction * (MoveSpeed * Time.deltaTime);
        }

        public bool IsRotating;
        public float RotatingSpeed;
        private void Rotate() {
            if (IsRotating)
                _transform.rotation *= Quaternion.Euler(0f, 0f, 1f * RotatingSpeed * Time.deltaTime);
        }
    }
}
