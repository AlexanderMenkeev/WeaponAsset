using System;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon {
    
    public class CoordinateSystem : MonoBehaviour {
        public AbstractWeapon Parent;
        public bool Move;
        public float Speed;
        public Vector3 Direction;
        
        private Transform _transform;
        private void Awake() {
            _transform = transform;
        }

        private void OnDestroy() {
            Parent.CoordinateSystems.Remove(this);
        }

        private void Update() {
            if (Move)
                _transform.position += Direction * (Speed * Time.deltaTime);
        }

        private void LateUpdate() {
            if (transform.childCount == 0)
                Destroy(gameObject);
        }
    }
}
