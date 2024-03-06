using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SharpNeat.Genomes.Neat;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.NEAT;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.ProjectileStatePattern;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.Weapon {
    /// <summary>
    /// Methods and variables for weapon system. Inherit from this abstract class to use.
    /// </summary>
    public abstract class AbstractWeapon : MonoBehaviour
    {
        
        [SerializeField] protected WeaponParamsSO _weaponSO;
        public Projectile ProjectilePrefab;
        public CoordinateSystem CoordinateSystemPrefab;
        public List<CoordinateSystem> CoordinateSystems;
        
        public GameObject TemporalObjects;
        [SerializeField] public Transform ProjectileSpawnPoint;
        
        
        [SerializeField] protected WeaponParams _weaponParamsLocal;
        [Tooltip("Do not change these stats in the editor, it will not have effect on the evolution algorithm.")]
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
                switch (_weaponParamsLocal.WeaponMode) {

                    case WeaponMode.MultiShot:
                        FireMultiShot();
                        break;
                    
                    case WeaponMode.Burst:
                        StartCoroutine(FireBurst());
                        
                        // Wait for FireBurst Coroutine to complete
                        yield return new WaitForSeconds(_weaponParamsLocal.ProjectilesInOneShot * _weaponParamsLocal.BurstRate);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                yield return new WaitForSeconds(_weaponParamsLocal.FireRate);
            }
        }


        
        public virtual void FireMultiShot() {
            
            float signX = (_weaponParamsLocal.FlipX) ? -1f : 1f;
            float signY = (_weaponParamsLocal.FlipY) ? -1f : 1f;
            CoordinateSystem localCoordinateSystem = CreateLocalCoordinateSystem();
            
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
                    
                projectile.SignX = (offset < 0 ? -1f : 1f) * signX;
                projectile.SignY = signY;

                Vector2 initialDirection = Quaternion.Euler(0, 0, _weaponParamsLocal.Angle * offset) * ProjectileSpawnPoint.up;
                projectile.InitialVelocity = initialDirection.normalized * _weaponParamsLocal.InitialSpeed;
            }
        }
        
        
        public virtual IEnumerator FireBurst() {
            
            float signX = (_weaponParamsLocal.FlipX) ? -1f : 1f;
            float signY = (_weaponParamsLocal.FlipY) ? -1f : 1f;
            CoordinateSystem localCoordinateSystem = CreateLocalCoordinateSystem();
            int projectileCount = _weaponParamsLocal.ProjectilesInOneShot;
            BurstMode burstMode = _weaponParamsLocal.BurstMode;
            
            List<float> offsets = new List<float>(projectileCount);
            if (projectileCount == 1)
                offsets.Add(0f);
            else {
                for (int i = 0; i < projectileCount; i++) 
                    offsets.Add(Mathf.Lerp(-1f, 1f, (float)i / (projectileCount - 1)));
            }
            
            int negIdx = projectileCount / 2 - 1; 
            int posIdx = projectileCount / 2;
            for (int i = 0; i < projectileCount; i++) {
                
                if (localCoordinateSystem == null)
                    yield break;
                
                Projectile projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity, localCoordinateSystem.transform).GetComponent<Projectile>();

                // initialize projectile with necessary data
                projectile.OriginTransform = localCoordinateSystem.transform;
                projectile.Box = GenomeStats.Box;

                projectile.WeaponParamsLocal = new WeaponParams(_weaponParamsLocal);
                projectile.WeaponSo = _weaponSO;

                float offset;
                Vector2 initialDirection;
                switch (burstMode) {

                    case BurstMode.Clockwise:
                        offset = offsets[projectileCount - 1 - i];
                        initialDirection = Quaternion.Euler(0, 0, _weaponParamsLocal.Angle * offset) * ProjectileSpawnPoint.up;
                        break;
                    
                    case BurstMode.CounterClockwise:
                        offset = offsets[i];
                        initialDirection = Quaternion.Euler(0, 0, _weaponParamsLocal.Angle * offset) * ProjectileSpawnPoint.up;
                        break;
                    
                    case BurstMode.Alternate:
                        if (i % 2 == 0) {
                            offset = offsets[posIdx];
                            posIdx++;
                        }
                        else {
                            offset = offsets[negIdx];
                            negIdx--;
                        }
                        initialDirection = Quaternion.Euler(0, 0, _weaponParamsLocal.Angle * offset) * ProjectileSpawnPoint.up;
                        break;
                    
                    case BurstMode.Straight:
                        if (i % 2 == 0) {
                            offset = offsets[posIdx];
                            posIdx++;
                        }
                        else {
                            offset = offsets[negIdx];
                            negIdx--;
                        }
                        initialDirection = ProjectileSpawnPoint.up;
                        break;
                    
                    case BurstMode.MaxMinAngle:
                        if (i % 2 == 0) {
                            offset = offsets[posIdx];
                            posIdx++;
                        }
                        else {
                            offset = offsets[negIdx];
                            negIdx--;
                        }
                        initialDirection = Quaternion.Euler(0, 0, _weaponParamsLocal.Angle * (offset < 0 ? -1f : 1f)) * ProjectileSpawnPoint.up;
                        break;

                    case BurstMode.Random:
                        offset = offsets[Random.Range(0, projectileCount)];
                        initialDirection = Quaternion.Euler(0, 0, _weaponParamsLocal.Angle * offset) * ProjectileSpawnPoint.up;
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                projectile.SignY = signY;
                projectile.SignX = (offset < 0 ? -1f : 1f) * signX;
                projectile.InitialVelocity = initialDirection.normalized * _weaponParamsLocal.InitialSpeed;
                
                yield return new WaitForSeconds(_weaponParamsLocal.BurstRate);
            }
        }
        
        
        
        private CoordinateSystem CreateLocalCoordinateSystem() {
            // Projectiles instantiated in the same shot or burst will use their localCoordinateSystem to calculate their local coordinates. 
            CoordinateSystem localCoordinateSystem = Instantiate(CoordinateSystemPrefab).GetComponent<CoordinateSystem>();
            localCoordinateSystem.Parent = this;
            
            // AbsolutePos => projectiles DO NOT move with the weapon
            // RelativePos => projectiles DO move with the weapon
            localCoordinateSystem.transform.parent = _weaponParamsLocal.PositioningMode switch {
                PositioningMode.AbsolutePos => TemporalObjects.transform,
                PositioningMode.RelativePos => ProjectileSpawnPoint.transform,
                _ => throw new ArgumentOutOfRangeException()
            };

            // localCoordinateSystem has the same rotation and position as ProjectileSpawnPoint
            localCoordinateSystem.transform.up = ProjectileSpawnPoint.up;            
            localCoordinateSystem.transform.right = ProjectileSpawnPoint.right;      
            localCoordinateSystem.transform.rotation = ProjectileSpawnPoint.rotation;
            localCoordinateSystem.transform.position = ProjectileSpawnPoint.position;

            CoordinateSystems.Add(localCoordinateSystem);
            return localCoordinateSystem;
        }


        public virtual void LaunchCoordinateSystems(float speed, Vector3 direction) {
            foreach (CoordinateSystem system in CoordinateSystems) {
                system.Speed = speed;
                system.Direction = direction;
                system.Move = true;
            }
        } 
        
        
        // Visualization of initial flight circle radius, reflection borders and network control distance
        private float _borderRayDirX, _borderRayDirY;
        private Vector2 _upperBorderRayDir, _lowerBorderRayDir;
        private void OnDrawGizmosSelected() {
            if (ProjectileSpawnPoint == null)
                return;
            
            switch (_weaponParamsLocal.Mode) {

                case ReflectionMode.CircleReflection:
                    Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.InitialFlightRadius);
                    Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * Mathf.Sqrt(2));
                    Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, _weaponParamsLocal.NNControlDistance * _weaponParamsLocal.ReflectiveCircleRadius);
                    break;
                
                case ReflectionMode.Polar:
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
                
                case ReflectionMode.RectangleReflection:
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
