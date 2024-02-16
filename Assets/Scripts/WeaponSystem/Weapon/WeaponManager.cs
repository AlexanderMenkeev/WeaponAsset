using System.Collections.Generic;
using SharpNeat.Genomes.Neat;
using UnityEngine;
using WeaponSystem.NEAT;

namespace WeaponSystem.Weapon {
    public class WeaponManager : MonoBehaviour
    {
        private Camera _camera;
    
        private EvolutionAlgorithm _evolutionAlgorithm;
        private int _numberOfWeapons;
        private IList<NeatGenome> _genomeList;
        [SerializeField] private List<GameObject> _weapons;
        
        // assigned from the editor
        [SerializeField] private GameObject _weaponPrefab;
        
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

            float stepY = (p11.y - p00.y) / 2f;
            float stepX = 2f * (p11.x - p00.x) / _numberOfWeapons;
            
            for (int i = 0; i < _numberOfWeapons / 2; i++) {
                Vector3 pos = new Vector3(p00.x + i * 0.9f * stepX + stepX * 0.75f, p11.y - stepY * 0.55f, 0);
                _weapons.Add(Instantiate(_weaponPrefab, pos, Quaternion.identity, transform));
            }
            
            int weaponsCount = _weapons.Count;
            for (int i = 0; i <_numberOfWeapons - weaponsCount; i++) {
                Vector3 pos = new Vector3(p00.x + i * 0.9f * stepX + stepX * 0.5f, p00.y + stepY * 0.55f, 0);
                _weapons.Add(Instantiate(_weaponPrefab, pos, Quaternion.identity, transform));
            }
        }

        private void InitializeWeapons() {
            for (int i = 0; i < _numberOfWeapons; i++) {
                Weapon weapon = _weapons[i].GetComponent<Weapon>();
                weapon.GenomeStats = new GenomeStats(_genomeList[i], _evolutionAlgorithm.Decoder, _evolutionAlgorithm.CppnGenomeFactory);
            }
        }
    
    

    }
}
