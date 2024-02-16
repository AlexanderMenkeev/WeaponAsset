using System.Collections;
using SODefinitions;
using UnityEngine;
using WeaponSystem.NEAT;
using WeaponSystem.ProjectileStatePattern;

namespace WeaponSystem.Weapon {
    public abstract class AbstractWeapon : MonoBehaviour
    {
        // assigned from the editor
        [SerializeField] protected WeaponParamsSO _weaponParamsGlobal;
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
            _weaponParamsGlobal.UpdateParamsEvent += InitializeParams;
        }

        protected virtual void OnDestroyFunc() {
            _weaponParamsGlobal.UpdateParamsEvent -= InitializeParams;
        }

        protected virtual void OnStartFunc() {
            FireCoroutine = StartCoroutine(FireProjectile());
        }

        protected void InitializeParams() {
            _weaponParamsLocal = new WeaponParams(_weaponParamsGlobal);
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
    
    
        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.InitialFlightRadius);
            Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * Mathf.Sqrt(2));
            Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.ReflectiveCircleRadius);
        }
    
    
    
    }
}
