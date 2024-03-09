using System.Collections.Generic;
using NEATProjectiles.Core.Scripts.SODefinitions;
using NEATProjectiles.Core.Scripts.WeaponSystem.NEAT;
using SharpNeat.Genomes.Neat;
using Unity.Mathematics;
using UnityEngine;

namespace NEATProjectiles.Core.Scripts.WeaponSystem {
    public class WeaponManager : MonoBehaviour
    {
        private Camera _camera;
    
        private EvolutionAlgorithm _evolutionAlgorithm;
        private int _numberOfWeapons;
        private IList<NeatGenome> _genomeList;
        [SerializeField] private List<GameObject> _weapons;
        
        // assign in the editor
        [SerializeField] private GameObject _weaponPrefab;
        [SerializeField] private WeaponParamsSO _weaponSo;
        
        private void Awake() {
            _camera = Camera.main;
        }
    
        private void Start() {
            _evolutionAlgorithm = EvolutionAlgorithm.Instance;
            _numberOfWeapons = _evolutionAlgorithm.PopulationSize;
        
            InstantiateWeapons();
            _genomeList = _evolutionAlgorithm.GenomeList;
            InitializeWeapons();
        
            _evolutionAlgorithm.NewGenEvent += InitializeWeapons;
        }

        private void OnDestroy() {
            _evolutionAlgorithm.NewGenEvent -= InitializeWeapons;
        }

        
        private void InstantiateWeapons() {
            Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            
            float horizontalSpacing = _weaponSo.NNControlDistance * math.SQRT2 * 2.2f;
            float marginY = _weaponSo.NNControlDistance * math.SQRT2;
                
            for (int i = 0; i < _numberOfWeapons / 2; i++) {
                Vector3 pos = new Vector3(p00.x + i * horizontalSpacing + horizontalSpacing, p11.y - marginY, 0);
                _weapons.Add(Instantiate(_weaponPrefab, pos, Quaternion.identity, transform));
            }
            
            int weaponsCount = _weapons.Count;
            for (int i = 0; i <_numberOfWeapons - weaponsCount; i++) {
                Vector3 pos = new Vector3(p00.x + i * horizontalSpacing + horizontalSpacing * 0.5f, p00.y + marginY, 0);
                _weapons.Add(Instantiate(_weaponPrefab, pos, Quaternion.identity, transform));
            }
        }

        
        private void InitializeWeapons() {
            RepositionWeapons();
            _weaponSo.DestroyProjectilesEvent?.Invoke();
            for (int i = 0; i < _numberOfWeapons; i++) {
                
                EvoWeapon weapon = _weapons[i].GetComponent<EvoWeapon>();
                if (weapon.FireCoroutine != null) 
                    StopCoroutine(weapon.FireCoroutine);
                
                weapon.GenomeStats = new GenomeStats(_genomeList[i], _evolutionAlgorithm.Decoder, _evolutionAlgorithm.CppnGenomeFactory);
                
                weapon.FireCoroutine = StartCoroutine(weapon.Fire());
            }
        }

        
        private void RepositionWeapons() {
            Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            
            float horizontalSpacing = _weaponSo.NNControlDistance * math.SQRT2 * 2.2f;
            float marginY = _weaponSo.NNControlDistance * math.SQRT2;

            for (int i = 0; i < _weapons.Count; i++)  {
                for (int j = 0; j < _numberOfWeapons / 2; j++) {
                    Vector3 pos = new Vector3(p00.x + j * horizontalSpacing + horizontalSpacing, p11.y - marginY, 0);
                    _weapons[j].transform.position = pos;
                }
                
                for (int j = 0; j <_numberOfWeapons - _numberOfWeapons / 2; j++) {
                    Vector3 pos = new Vector3(p00.x + j * horizontalSpacing + horizontalSpacing * 0.5f, p00.y + marginY, 0);
                    _weapons[j + _numberOfWeapons / 2].transform.position = pos;
                }
            }
            
        }
        
    }
}
