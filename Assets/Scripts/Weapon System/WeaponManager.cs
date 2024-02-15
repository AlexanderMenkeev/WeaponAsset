using System;
using System.Collections.Generic;
using SharpNeat.Genomes.Neat;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Camera _camera;
    
    private EvolutionAlgorithm _evolutionAlgorithm;
    [SerializeField] private GameObject _weaponPrefab;
    [SerializeField] private List<GameObject> _weapons;

    private int _numberOfWeapons;
    
    private IList<NeatGenome> _genomeList;

    private void Awake()
    {
        _camera = Camera.main;
    }
    
    private void Start()
    {
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

    private void InstantiateWeapons()
    {
        Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));

        float stepX = (p11.x - p00.x) / (_numberOfWeapons * 0.5f);
        
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = new Vector3(p00.x + i * stepX + stepX * 0.5f, p00.y + stepX * 0.8f, 0);
            _weapons.Add(Instantiate(_weaponPrefab, pos, Quaternion.identity, transform));
        }
        
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = new Vector3(p00.x + i * stepX + stepX * 0.5f, p11.y - stepX * 0.8f, 0);
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
