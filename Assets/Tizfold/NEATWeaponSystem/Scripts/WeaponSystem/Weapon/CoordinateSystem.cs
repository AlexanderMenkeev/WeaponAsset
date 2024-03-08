using System;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon {
    
    public class CoordinateSystem : MonoBehaviour {
        
        public AbstractWeapon Parent;
        
        private Transform _transform;
        private void Awake() {
            _transform = transform;
            IsMoving = false;
            IsRotating = false;
        }

        private void OnDestroy() {
            Parent.CoordinateSystems.Remove(this);
        }

        private void Update() {
            Move();
            Rotate();
        }

        // if all child projectiles were destroyed => destroy this CoordinateSystem
        private void LateUpdate() {
            if (transform.childCount == 0)
                Destroy(gameObject);
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
