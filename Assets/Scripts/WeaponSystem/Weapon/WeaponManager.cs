using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SharpNeat.Genomes.Neat;
using SODefinitions;
using Unity.Mathematics;
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
            for (int i = 0; i < _numberOfWeapons; i++) {
                Weapon weapon = _weapons[i].GetComponent<Weapon>();
                weapon.GenomeStats = new GenomeStats(_genomeList[i], _evolutionAlgorithm.Decoder, _evolutionAlgorithm.CppnGenomeFactory);
            }
        }




    }
}
