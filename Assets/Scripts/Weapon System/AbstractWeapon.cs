using System;
using System.Collections;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.HyperNeat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using Unity.Mathematics;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public GameObject TemporalObjects;
    public NeatGenome ProjectileGenome;
    public Transform ProjectileSpawnPoint;

    public PlayerScript Player;
    
    [SerializeField] protected WeaponSO _weaponParams;
    
    [Header("Projectile controls")]
    [SerializeField] protected Vector2 _size;
    [SerializeField] [Range(3f, 12f)] protected float _lifespan;
    
    [Header("Pattern controls")]
    [SerializeField] [Range(1f, 12f)] protected float _minSpeed;
    [SerializeField] [Range(13f, 25f)] protected float _maxSpeed;
    [SerializeField] [Range(1f, 5f)] protected float _minForce;
    [SerializeField] [Range(6f, 15f)] protected float _maxForce;
    [SerializeField] [Range(1f, 10f)] protected float _maxDistance;
    [SerializeField] [Range(0.1f, 1f)] protected float _fireRate;
    [SerializeField] [Range(1, 20)] protected int _projectilesInOneShot;
    [SerializeField] protected bool _flipY;
    [SerializeField] [Range(math.SQRT2, 2f)] protected float _reflectiveCircleRadios;
    
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
    protected ISignalArray _inputArr;
    protected ISignalArray _outputArr;

    public virtual void InitializeParams() {
        _size = _weaponParams.Size;
        _lifespan = _weaponParams.Lifespan;
        _minSpeed = _weaponParams.MinSpeed;
        _maxSpeed = _weaponParams.MaxSpeed;
        _minForce = _weaponParams.MinForce;
        _maxForce = _weaponParams.MaxForce;
        _maxDistance = _weaponParams.MaxDistance;
        _fireRate = _weaponParams.FireRate;
        _flipY = _weaponParams.FlipY;
        _projectilesInOneShot = _weaponParams.ProjectilesInOneShot;
        _reflectiveCircleRadios = _weaponParams.ReflectiveCircleRadios;
    }
    
    public Coroutine FireCoroutine;
    public virtual IEnumerator FireProjectile()
    {
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
                Projectile newProjectileScript = newProjectile.GetComponent<Projectile>();

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

            }

            yield return new WaitForSeconds(_fireRate);
        }
    }
    
    
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, _maxDistance * Mathf.Sqrt(2));
        Gizmos.DrawWireSphere(transform.position, _maxDistance * _reflectiveCircleRadios );
        Gizmos.DrawRay(transform.position, transform.right);
    }
    
    
    
}
