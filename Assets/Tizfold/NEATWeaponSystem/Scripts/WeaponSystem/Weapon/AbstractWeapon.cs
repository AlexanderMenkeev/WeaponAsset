using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using SharpNeat.Genomes.Neat;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.NEAT;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon {
    /// <summary>
    /// Methods and variables for weapon system. Inherit from this abstract class to use.
    /// </summary>
    public abstract class AbstractWeapon : MonoBehaviour
    {
        #region Assign in editor
        
        [SerializeField] protected WeaponParamsSO _weaponSO;
        public GameObject ProjectilePrefab;
        
        public GameObject TemporalObjects;
        [SerializeField] public Transform ProjectileSpawnPoint;
        
        #endregion
        
        [SerializeField] protected WeaponParams _weaponParamsLocal;
        [Tooltip("Do not change these stats in editor, it will not have effect on evolution algorithm.")]
        public GenomeStats GenomeStats;
        
        
        protected virtual void InitializeParams() {
            _weaponParamsLocal = new WeaponParams(_weaponSO);
            TryToLoadGenomeFromSO();
        }
        
        
        private void TryToLoadGenomeFromSO() {
            if (_weaponSO.GenomeXml == null) 
                return;
            
            XmlDocument genomeXml = new XmlDocument();
            genomeXml.LoadXml(_weaponSO.GenomeXml.text);
            
            List<NeatGenome> genomeList = NeatGenomeXmlIO.LoadCompleteGenomeList(genomeXml, true, EvolutionAlgorithm.Instance.CppnGenomeFactory);
            GenomeStats = new GenomeStats(genomeList[0], EvolutionAlgorithm.Instance.Decoder, EvolutionAlgorithm.Instance.CppnGenomeFactory);
        }
        
        
        public Coroutine FireCoroutine;
        
        /// <summary>
        /// To start coroutine => FireCoroutine = StartCoroutine(Fire()); <br />
        /// To stop coroutine => StopCoroutine(FireCoroutine);
        /// </summary>
        public virtual IEnumerator Fire() {
            while (true) {
                FireShot();
                yield return new WaitForSeconds(_weaponParamsLocal.FireRate);
            }
        }

        
        /// <summary>
        /// Fires one shot of projectiles. Use this method, if you want to have more control over shooting behavior.
        /// (For example, you may want to invoke FireShot on ButtonPress or Click)
        /// </summary>
        public virtual void FireShot() {
            float signY = (_weaponParamsLocal.FlipY) ? -1 : 1;
                
            // Projectiles instantiated in the same shot will use their localCoordinateSystem to calculate their local coordinates. 
            GameObject localCoordinateSystem = new GameObject("Local Coordinate System");
            
            // localCoordinateSystem has the same rotation and position as ProjectileSpawnPoint
            localCoordinateSystem.transform.parent = TemporalObjects.transform;
            localCoordinateSystem.transform.up = ProjectileSpawnPoint.up;            
            localCoordinateSystem.transform.right = ProjectileSpawnPoint.right;      
            localCoordinateSystem.transform.rotation = ProjectileSpawnPoint.rotation;
            localCoordinateSystem.transform.position = ProjectileSpawnPoint.position;
            
            for (int i = 0; i < _weaponParamsLocal.ProjectilesInOneShot; i++) {
                // Offset for InitialFlight
                float offset = 0;
                if (_weaponParamsLocal.ProjectilesInOneShot != 1)
                    offset = Mathf.Lerp(-1f, 1f, (float)i / (_weaponParamsLocal.ProjectilesInOneShot - 1));

                Projectile projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity, localCoordinateSystem.transform).GetComponent<Projectile>();

                // initialize projectile with necessary data
                projectile.OriginTransform = localCoordinateSystem.transform;
                projectile.Box = GenomeStats.Box;

                projectile.WeaponParamsLocal = new WeaponParams(_weaponParamsLocal);
                projectile.WeaponSo = _weaponSO;
                    
                projectile.SignX = offset < 0 ? -1f : 1f;
                projectile.SignY = signY;
                    
                Vector2 initialDirection = Quaternion.Euler(0, 0, _weaponParamsLocal.Angle * offset) * ProjectileSpawnPoint.up;
                projectile.InitialVelocity = initialDirection.normalized * _weaponParamsLocal.InitialSpeed;
            }
        }
    
    
        
        // Visualization of initial flight circle radius, reflection borders and network control distance
        private float _borderRayDirX, _borderRayDirY;
        private Vector2 _upperBorderRayDir, _lowerBorderRayDir;
        private void OnDrawGizmosSelected() {
            if (ProjectileSpawnPoint == null)
                return;
            
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
