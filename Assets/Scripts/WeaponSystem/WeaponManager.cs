using System.Collections.Generic;
using SharpNeat.Genomes.Neat;
using UnityEngine;

namespace WeaponSystem {
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
            _genomeList = _evolutionAlgorithm.GetComponent<EvolutionAlgorithm>().GenomeList;
            InitializeWeapons();
        
            _evolutionAlgorithm.NewGenEvent += UpdateWeapons;
        }

        private void OnDestroy() {
            _evolutionAlgorithm.NewGenEvent -= UpdateWeapons;
        }

        private void InstantiateWeapons() {
            Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));

            float stepY = (p11.y - p00.y) / 2f;
            float stepX = (p11.x - p00.x) / (_numberOfWeapons / 2f);
            
            for (int i = 0; i < _numberOfWeapons / 2; i++) {
                Vector3 pos = new Vector3(p00.x + i * 0.9f * stepX + stepX * 0.25f, p00.y + stepY * 0.55f, 0);
                _weapons.Add(Instantiate(_weaponPrefab, pos, Quaternion.identity, transform));
            }

            int weaponsCount = _weapons.Count;
            for (int i = 0; i < _numberOfWeapons - weaponsCount; i++) {
                Vector3 pos = new Vector3(p00.x + i * 0.9f * stepX + stepX * 0.75f, p11.y - stepY * 0.55f, 0);
                _weapons.Add(Instantiate(_weaponPrefab, pos, Quaternion.identity, transform));
            }
        }

        private void InitializeWeapons()
        {
            for (int i = 0; i < _numberOfWeapons; i++)
            {
                Weapon weapon = _weapons[i].GetComponent<Weapon>();
            
                weapon.ProjectileGenome = _genomeList[i];
                weapon.ID = _genomeList[i].Id;
                weapon.BirthGeneration = _genomeList[i].BirthGeneration;
                weapon.Connections = _genomeList[i].Complexity;
                weapon.IsEvaluated = _genomeList[i].EvaluationInfo.IsEvaluated;
            
                weapon.Nodes = _genomeList[i].NodeList.Count;
            
                weapon.Decoder = _evolutionAlgorithm.GetComponent<EvolutionAlgorithm>().Decoder;
                weapon.Factory = _evolutionAlgorithm.GetComponent<EvolutionAlgorithm>().CppnGenomeFactory;
            }
        }
        private void UpdateWeapons()
        {
            for (int i = 0; i < _numberOfWeapons; i++)
            {
                Weapon weapon = _weapons[i].GetComponent<Weapon>();
            
                weapon.ProjectileGenome = _genomeList[i];
                weapon.ID = _genomeList[i].Id;
                weapon.BirthGeneration = _genomeList[i].BirthGeneration;
                weapon.Connections = _genomeList[i].Complexity;
                weapon.IsEvaluated = _genomeList[i].EvaluationInfo.IsEvaluated;
            
                weapon.Nodes = _genomeList[i].NodeList.Count;
            }
        }
    
    

    }
}
