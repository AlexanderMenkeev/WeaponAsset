using System;
using System.Collections;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.HyperNeat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using SODefinitions;
using Unity.Mathematics;
using UnityEngine;
using WeaponSystem.ProjectileStatePattern;

namespace WeaponSystem {
    public abstract class AbstractWeapon : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public GameObject TemporalObjects;
        public NeatGenome ProjectileGenome;
        public Transform ProjectileSpawnPoint;
    
        [SerializeField] protected WeaponParamsSO _weaponParamsGlobal;
        [SerializeField] protected WeaponParams _weaponParamsLocal;
       
    
        [Header("NeatGenome stats")]
        public uint BirthGeneration;
        public uint ID { get; set; }
        public double Connections;
        public int Nodes;
    
        public bool IsEvaluated;
        public bool Save;
        public bool ChangeOnBestGenome;
    
        public NeatGenomeDecoder Decoder;
        public CppnGenomeFactory Factory;
    
        protected IBlackBox _box;
        
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

        public void InitializeParams() {
            _weaponParamsLocal = new WeaponParams(_weaponParamsGlobal);
        }
    
        public Coroutine FireCoroutine;
        public virtual IEnumerator FireProjectile()
        {
            while (true) {
                _box = Decoder.Decode(ProjectileGenome);
                float signY = (_weaponParamsLocal.FlipY) ? -1 : 1;
            
                GameObject localCoordinateSystem = new GameObject("Local Coordinate System for projectiles");
                localCoordinateSystem.transform.parent = TemporalObjects.transform;
                Transform localCoordinateSystemTransform = localCoordinateSystem.transform;
                localCoordinateSystemTransform.up = ProjectileSpawnPoint.up;            
                localCoordinateSystemTransform.right = ProjectileSpawnPoint.right;      
                localCoordinateSystemTransform.rotation = ProjectileSpawnPoint.rotation;
                localCoordinateSystemTransform.position = ProjectileSpawnPoint.position;
            
                for (int i = 0; i < _weaponParamsLocal.ProjectilesInOneShot; i++) {

                    float signX = 0;
                    if (_weaponParamsLocal.ProjectilesInOneShot != 1)
                        signX = Mathf.Lerp(-1f, 1f, (float)i / (_weaponParamsLocal.ProjectilesInOneShot - 1));

                    GameObject projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity, localCoordinateSystemTransform);
                    Projectile projectileScript = projectile.GetComponent<Projectile>();

                    projectileScript.ParentTransform = localCoordinateSystemTransform;
                    projectileScript.Box = _box;

                    projectileScript.WeaponParamsLocal = new WeaponParams(_weaponParamsLocal);
                    
                
                    projectileScript.SignX = signX < 0 ? -1f : 1f;
                    projectileScript.SignY = signY;
                    
                
                    Vector2 initialDirection = Quaternion.Euler(0, 0, 45 * signX) * transform.right;
                
                    projectileScript.InitialVelocity = initialDirection.normalized * _weaponParamsLocal.MinSpeed;

                }

                yield return new WaitForSeconds(_weaponParamsLocal.FireRate);
            }
        }
    
    
        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * Mathf.Sqrt(2));
            Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.ReflectiveCircleRadius);
        }
    
    
    
    }
}
