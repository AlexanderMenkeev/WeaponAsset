using System;
using System.Collections;
using System.Collections.Generic;
using SharpNeat.Genomes.Neat;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSpawner : MonoBehaviour {
    [SerializeField] private WeaponItem _weaponItemPrefab;
    [SerializeField] private List<WeaponItem> _weaponItems;
    
    [SerializeField] private GameObject _pickUpCanvas;
    [SerializeField] private GameObject _joysticksCanvas;
    [SerializeField] private CanvasDemoWeapon _canvasDemoWeapon;
    [SerializeField] private GameSceneWeapon _gameSceneWeapon;
    [SerializeField] private Slider _distanceSlider;
    [SerializeField] private Toggle _flip;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _dismissButton;
    
    private Camera _camera;
    
    private IList<NeatGenome> _genomeList;
    private int _numberOfWeapons;
    private EvolutionAlgorithm _evolutionAlgorithm;
    private void Awake()
    {
        _camera = Camera.main;
    }
    
    
    
    private void InstantiateWeaponItems()
    {
        Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));

        float stepX = (p11.x - p00.x) / _numberOfWeapons;
        
        for (int i = 0; i < _numberOfWeapons; i++)
        {
            Vector3 pos = new Vector3(p00.x + i * stepX + stepX * 0.5f, p00.y + stepX * 0.8f, 0);
            WeaponItem item = Instantiate(_weaponItemPrefab, pos, Quaternion.identity, transform);
            item.JoysticksCanvas = _joysticksCanvas;
            item.PickUpCanvas = _pickUpCanvas;
            item.DemoWeapon = _canvasDemoWeapon;
            item.SceneWeapon = _gameSceneWeapon;
            item.DistanceSlider = _distanceSlider;
            item.Flip = _flip;
            item.AcceptButton = _acceptButton;
            item.DismissButton = _dismissButton;
            
            item.Decoder = _evolutionAlgorithm.GetComponent<EvolutionAlgorithm>().Decoder;
            item.Factory = _evolutionAlgorithm.GetComponent<EvolutionAlgorithm>().CppnGenomeFactory;
            
            _weaponItems.Add(item);
        }
    }


    private void Start() {
        _evolutionAlgorithm = EvolutionAlgorithm.Instance;
        _numberOfWeapons = _evolutionAlgorithm.PopulationSize;
        _genomeList = _evolutionAlgorithm.GetComponent<EvolutionAlgorithm>().GenomeList;
        
        InstantiateWeaponItems();
        
        UpdateWeapons();
        
        _evolutionAlgorithm.NewGenEvent += UpdateWeapons;

        _evolutionAlgorithm.CreateNewGeneration();
    }

    private void OnDestroy() {
        _evolutionAlgorithm.NewGenEvent -= UpdateWeapons;
    }

    private void UpdateWeapons()
    {
        for (int i = 0; i < _numberOfWeapons; i++)
        {
            WeaponItem item = _weaponItems[i];
            
            item.ProjectileGenome = _genomeList[i];
            item.ID = _genomeList[i].Id;
            item.BirthGeneration = _genomeList[i].BirthGeneration;
            item.Connections = _genomeList[i].Complexity;
            item.IsEvaluated = _genomeList[i].EvaluationInfo.IsEvaluated;
            
            item.Nodes = _genomeList[i].NodeList.Count;
        }
    }
    
    
    
}
