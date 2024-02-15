using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using SharpNeat.Genomes.Neat;
using UnityEngine;

public class CanvasDemoWeapon : AbstractWeapon
{
    private void Awake()
    {
        TemporalObjects = GameObject.Find("TemporalObjectsUI");
        ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
        InitializeParams();
        _weaponParams.UpdateParamsEvent += InitializeParams;
        OnChangeDemoWeaponEvent += RestartCoroutine;
    }

    private void OnDestroy()
    {
        _weaponParams.UpdateParamsEvent -= InitializeParams;
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
            float signY = (_flipY) ? -1 : 1;
            
            GameObject localCoordinateSystem = new GameObject("Local Coordinate System for projectiles");
            localCoordinateSystem.transform.parent = TemporalObjects.transform;
            Transform parentTransform = localCoordinateSystem.transform;
            parentTransform.up = ProjectileSpawnPoint.up;            
            parentTransform.right = ProjectileSpawnPoint.right;      
            parentTransform.rotation = ProjectileSpawnPoint.rotation;
            parentTransform.position = ProjectileSpawnPoint.position;
            
            for (int i = 0; i < _projectilesInOneShot; i++) {

                float signX = 0;
                if (_projectilesInOneShot != 1)
                    signX = Mathf.Lerp(-1f, 1f, (float)i / (_projectilesInOneShot - 1));

                GameObject newProjectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity, parentTransform);
                UIProjectile newProjectileScript = newProjectile.GetComponent<UIProjectile>();

                newProjectileScript.ParentTransform = parentTransform;
                newProjectileScript.Box = _box;

                
                newProjectileScript.transform.localScale = new Vector3(_size.x, _size.y, 1);
                newProjectileScript.Lifespan = _lifespan;
                newProjectileScript.MinSpeed = _minSpeed;
                newProjectileScript.MaxSpeed = _maxSpeed;
                newProjectileScript.MinForce = _minForce;
                newProjectileScript.MaxForce = _maxForce;
                newProjectileScript.NeuralNetworkControlDistance = _maxDistance;
                    
                
                newProjectileScript.SignX = signX < 0 ? -1f : 1f;
                newProjectileScript.SignY = signY;

                
                Vector2 initialDirection = Quaternion.Euler(0, 0, 45 * signX) * transform.right;
                
                newProjectileScript.InitialVelocity = initialDirection.normalized * _minSpeed;
                newProjectileScript.ReflectiveCircleRadius = _reflectiveCircleRadios;
                
                
                // this line is the difference between AbstractWeapon.FireProjectile and this.FireProjectile
                newProjectileScript.DemoWeapon = this;

            }

            yield return new WaitForSeconds(_fireRate);
        }
    }

    
    




}
