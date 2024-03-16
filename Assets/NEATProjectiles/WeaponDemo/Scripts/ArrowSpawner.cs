using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeatProjectiles.WeaponDemo.Scripts {
    public class ArrowSpawner : MonoBehaviour
    {
        [Tooltip ("Cell dimensions in units")]
        [SerializeField] private Vector2 _cellDimensions;

        [Tooltip ("Prefab to generate grid with")]
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private Vector2 _arrowDimensions;
        public List<GameObject> ArrowList = new List<GameObject>();
        
        [HideInInspector] [SerializeField] private Camera _camera;
        [HideInInspector] [SerializeField] private DemoWeapon _demoWeapon;
        private void Awake() {
            _camera = Camera.main;
            _demoWeapon = FindObjectOfType<DemoWeapon>();
            // _arrowPrefab.loca
        }
        
        
        [Header("Arrow spawn")] 
        [SerializeField] [Range(0f, 5f)] private float _xMargin;
        [SerializeField] [Range(0f, 5f)] private float _yMargin;
        public void CreateForceMap() {
            Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));

            p00 += new Vector2(_xMargin, _yMargin);
            p11 -= new Vector2(_xMargin, _yMargin);

            float horizontalSpace = p11.x - p00.x;
            float verticalSpace = p11.y - p00.y;
            
            int columnsCount = Mathf.CeilToInt(horizontalSpace / _cellDimensions.x);
            int rowsCount = Mathf.CeilToInt(verticalSpace / _cellDimensions.y);
            
            for (int row = 0; row < rowsCount; row++) {
                for (int col = 0; col < columnsCount; col++) {
                    ForceArrow arrow = Instantiate(_arrowPrefab, _demoWeapon.ProjectileSpawnPoint.transform).GetComponent<ForceArrow>();
                    arrow.name = "Arrow" + row + col;
                    arrow.transform.position = new Vector3(
                        p00.x + col * _cellDimensions.x + _cellDimensions.x * 0.5f,
                        p00.y + row * _cellDimensions.y + _cellDimensions.y * 0.5f);
                    arrow.ArrowSpawner = this;
                    
                    ArrowList.Add(arrow.gameObject);
                }
            }
            
        }
        
        
        
        
    }
}
