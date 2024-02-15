using System.Collections;
using System.Collections.Generic;
using System.Xml;
using SharpNeat.Genomes.Neat;
using UI;
using UnityEngine;
using WeaponSystem.ProjectileStatePattern;

namespace WeaponSystem {
    public class CanvasDemoWeapon : AbstractWeapon
    {
        private void Awake()
        {
            TemporalObjects = GameObject.Find("TemporalObjectsUI");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
            InitializeParams();
            _weaponParamsGlobal.UpdateParamsEvent += InitializeParams;
            OnChangeDemoWeaponEvent += RestartCoroutine;
        }

        private void OnDestroy()
        {
            _weaponParamsGlobal.UpdateParamsEvent -= InitializeParams;
            OnChangeDemoWeaponEvent -= RestartCoroutine;
        }

        private void LoadGenome() {
            TextAsset textAsset = (TextAsset) Resources.Load("BestGenome");  
            XmlDocument genomeXml = new XmlDocument();
            genomeXml.LoadXml(textAsset.text);
        
            List<NeatGenome> genomeList = NeatGenomeXmlIO.LoadCompleteGenomeList(genomeXml, true, Factory);
            ProjectileGenome = genomeList[0];
        }
    
    
        public delegate void DestroyStrayProjectiles();
        public DestroyStrayProjectiles OnChangeDemoWeaponEvent;
        private void OnDisable() {
            OnChangeDemoWeaponEvent?.Invoke();
            StopCoroutine(FireCoroutine);
        }

        private void OnEnable() {
            FireCoroutine = StartCoroutine(FireProjectile());
        }

        private void RestartCoroutine() {
            if (!isActiveAndEnabled)
                return;
        
            StopCoroutine(FireCoroutine);
            FireCoroutine = StartCoroutine(FireProjectile());
        }

        public override IEnumerator FireProjectile()
        {
            yield return new WaitForSeconds(0.1f);
            
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

                    //projectileScript.Demo = this;
                    
                }

                yield return new WaitForSeconds(_weaponParamsLocal.FireRate);
            }
        }

    
    




    }
}
