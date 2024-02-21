using System;
using System.Collections;
using SODefinitions;
using UnityEditor;
using UnityEngine;
using WeaponSystem.NEAT;
using WeaponSystem.ProjectileStatePattern;

namespace WeaponSystem.Weapon {
    public abstract class AbstractWeapon : MonoBehaviour
    {
        // assigned from the editor
        [SerializeField] protected WeaponParamsSO _weaponSO;
        public GameObject ProjectilePrefab;
        
        [SerializeField] protected WeaponParams _weaponParamsLocal;
        
        // initialized from WeaponManager
        public GenomeStats GenomeStats;
        
        [Header("Genome controls")]
        public bool Save;
        public bool LoadGenome;
        
        [HideInInspector] public GameObject TemporalObjects;
        [HideInInspector] public Transform ProjectileSpawnPoint;
        
        protected virtual void OnAwakeFunc() {
            TemporalObjects = GameObject.FindWithTag("TemporalObjects");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
            InitializeParams();
            
            
            _weaponSO.UpdateParamsEvent += InitializeParams;
        }

        protected virtual void OnDestroyFunc() {
            _weaponSO.UpdateParamsEvent -= InitializeParams;
        }

        protected virtual void OnStartFunc() {
            FireCoroutine = StartCoroutine(FireProjectile());
        }

        private void InitializeParams() {
            _weaponParamsLocal = new WeaponParams(_weaponSO);
        }
    
        public Coroutine FireCoroutine;
        public virtual IEnumerator FireProjectile()
        {
            while (true) {
                float signY = (_weaponParamsLocal.FlipY) ? -1 : 1;
                
                GameObject localCoordinateSystem = new GameObject("Local Coordinate System");
                localCoordinateSystem.transform.parent = TemporalObjects.transform;
                localCoordinateSystem.transform.up = ProjectileSpawnPoint.up;            
                localCoordinateSystem.transform.right = ProjectileSpawnPoint.right;      
                localCoordinateSystem.transform.rotation = ProjectileSpawnPoint.rotation;
                localCoordinateSystem.transform.position = ProjectileSpawnPoint.position;
            
                for (int i = 0; i < _weaponParamsLocal.ProjectilesInOneShot; i++) {
                    // offset for InitialFlight
                    float offset = 0;
                    if (_weaponParamsLocal.ProjectilesInOneShot != 1)
                        offset = Mathf.Lerp(-1f, 1f, (float)i / (_weaponParamsLocal.ProjectilesInOneShot - 1));

                    GameObject projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity, localCoordinateSystem.transform);
                    Projectile projectileScript = projectile.GetComponent<Projectile>();

                    projectileScript.OriginTransform = localCoordinateSystem.transform;
                    projectileScript.Box = GenomeStats.Box;

                    projectileScript.WeaponParamsLocal = new WeaponParams(_weaponParamsLocal);
                    
                    projectileScript.SignX = offset < 0 ? -1f : 1f;
                    projectileScript.SignY = signY;
                    
                    Vector2 initialDirection = Quaternion.Euler(0, 0, _weaponParamsLocal.Angle * offset) * transform.right;
                    projectileScript.InitialVelocity = initialDirection.normalized * _weaponParamsLocal.InitialSpeed;
                }

                yield return new WaitForSeconds(_weaponParamsLocal.FireRate);
            }
        }
    
    
        private float _borderRayDirX, _borderRayDirY;
        private Vector2 _upperBorderRayDir, _lowerBorderRayDir;
        private void OnDrawGizmosSelected() {
            switch (_weaponParamsLocal.Mode) {

                case ProjectileMode.CircleReflection:
                    Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.InitialFlightRadius);
                    Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * Mathf.Sqrt(2));
                    Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.ReflectiveCircleRadius);
                    break;
                
                case ProjectileMode.Polar:
                    
                    _borderRayDirX = _weaponParamsLocal.NNControlDistance * Mathf.Sin(_weaponParamsLocal.MaxPolarAngleDeg * Mathf.Deg2Rad);
                    _borderRayDirY = _weaponParamsLocal.NNControlDistance * Mathf.Cos(_weaponParamsLocal.MaxPolarAngleDeg * Mathf.Deg2Rad);
                    
                    _upperBorderRayDir= ProjectileSpawnPoint.transform.TransformVector(new Vector2(- _borderRayDirX, _borderRayDirY)).normalized;
                    _lowerBorderRayDir = ProjectileSpawnPoint.transform.TransformVector(new Vector2(_borderRayDirX, _borderRayDirY)).normalized;
                    _upperBorderRayDir *= _weaponParamsLocal.NNControlDistance * Mathf.Sqrt(2);
                    _lowerBorderRayDir *= _weaponParamsLocal.NNControlDistance * Mathf.Sqrt(2);
                    
                    Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.InitialFlightRadius);
                    Gizmos.DrawRay(ProjectileSpawnPoint.position, _upperBorderRayDir);
                    Gizmos.DrawRay(ProjectileSpawnPoint.position, _lowerBorderRayDir);
                    
                    
                    
                    break;


                case ProjectileMode.RectangleReflection:
                    float maxX = _weaponParamsLocal.RectDimensions.x * _weaponParamsLocal.NNControlDistance;
                    float maxY = _weaponParamsLocal.RectDimensions.y * _weaponParamsLocal.NNControlDistance;
                    
                    Gizmos.DrawWireCube(ProjectileSpawnPoint.position, new Vector2(maxY * 2f, maxX * 2f));
                    Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.InitialFlightRadius);
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
    
    
    }
}
