using System.Collections.Generic;
using SharpNeat.Genomes.Neat;
using Tizfold.NEATWeaponSystem.Scripts.EvolutionScene;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.NEAT;
using UnityEngine;
using UnityEngine.UI;

namespace Tizfold.NEATWeaponSystem.Scripts.WeaponSystem {
    public class WeaponSpawner : MonoBehaviour {
        
        // assigned from the editor
        [SerializeField] private GameObject _pickUpCanvas;
        [SerializeField] private GameObject _joysticksCanvas;
        [SerializeField] private CanvasWeapon _canvasWeapon;
        [SerializeField] private GameWeapon _gameWeapon;
        [SerializeField] private Slider _distanceSlider;
        [SerializeField] private Toggle _flip;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _dismissButton;

        [SerializeField] private WeaponParamsSO _weaponSo;
        [SerializeField] private WeaponItem _weaponItemPrefab;
        
        
        private Camera _camera;
        private void Awake() {
            _camera = Camera.main;
        }
    
        private EvolutionAlgorithm _evolutionAlgorithm;
        private int _numberOfWeapons;
        private IList<NeatGenome> _genomeList;
        [SerializeField] private List<WeaponItem> _weaponItems;
        private void Start() {
            _evolutionAlgorithm = EvolutionAlgorithm.Instance;
            _numberOfWeapons = _evolutionAlgorithm.PopulationSize;
        
            InstantiateWeaponItems();
            _genomeList = _evolutionAlgorithm.GenomeList;
            InitializeWeapons();
        
            _evolutionAlgorithm.NewGenEvent += InitializeWeapons;
        }

        private void OnDestroy() {
            _evolutionAlgorithm.NewGenEvent -= InitializeWeapons;
        }
        
        private void InstantiateWeaponItems() {
            Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));

            float stepX = (p11.x - p00.x) / _numberOfWeapons;
        
            for (int i = 0; i < _numberOfWeapons; i++) {
                
                Vector3 pos = new Vector3(p00.x + i * stepX + stepX * 0.5f, p00.y + stepX * 0.8f, 0);
                WeaponItem item = Instantiate(_weaponItemPrefab, pos, Quaternion.identity, transform);
                item.WeaponSO = _weaponSo;
                item.JoysticksCanvas = _joysticksCanvas;
                item.PickUpCanvas = _pickUpCanvas;
                item.CanvasWeapon = _canvasWeapon;
                item.GameWeapon = _gameWeapon;
                item.DistanceSlider = _distanceSlider;
                item.Flip = _flip;
                item.AcceptButton = _acceptButton;
                item.DismissButton = _dismissButton;
            
                _weaponItems.Add(item);
            }
        }

        private void InitializeWeapons() {
            _weaponSo.DestroyProjectilesEvent?.Invoke();
            
            for (int i = 0; i < _numberOfWeapons; i++) {
                WeaponItem weaponItem = _weaponItems[i].GetComponent<WeaponItem>();
                weaponItem.GenomeStats = new GenomeStats(_genomeList[i], _evolutionAlgorithm.Decoder, _evolutionAlgorithm.CppnGenomeFactory);
            }
        }
    
    
    }
}
