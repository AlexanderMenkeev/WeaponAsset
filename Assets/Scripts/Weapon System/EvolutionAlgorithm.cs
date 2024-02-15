using System;
using System.Collections;
using System.Collections.Generic;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.HyperNeat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using UnityEngine;
using Random = UnityEngine.Random;


public class EvolutionAlgorithm : MonoBehaviour
{
    public IList<NeatGenome> GenomeList;

    public event Action NewGenEvent;
    public NeatGenomeDecoder Decoder;
    public CppnGenomeFactory CppnGenomeFactory;
    public NetworkActivationScheme ActivationScheme;
    public IActivationFunctionLibrary ActivationFunctionLib;

    [field: SerializeField] public int PopulationSize { private set; get; }
    [field: SerializeField] public uint Generation { private set; get; }
    [field: SerializeField] public int InputCount { private set; get; }
    [field: SerializeField] public int OutputCount { private set; get; }
    [field: SerializeField] public int CloneOffspringCount { private set; get; }
    [field: SerializeField] public int SexualOffspringCount { private set; get; }
    
    public static EvolutionAlgorithm Instance;
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        
        InitializeGenomeParams();
        InitializeEvolutionAlgorithm();
    }
    
    private void InitializeGenomeParams() {
        PopulationSize = 8;
        ActivationScheme = NetworkActivationScheme.CreateAcyclicScheme();
        Generation = 0;
        InputCount = 3;
        OutputCount = 7;
        CloneOffspringCount = PopulationSize - 2;
        SexualOffspringCount = 2;
    }
    private void InitializeEvolutionAlgorithm() {
        Decoder = new NeatGenomeDecoder(ActivationScheme);
        
        List<ActivationFunctionInfo> fnList = new List<ActivationFunctionInfo>(4);
        fnList.Add(new ActivationFunctionInfo(0, 0.1, Linear.__DefaultInstance));
        fnList.Add(new ActivationFunctionInfo(1, 0.3, BipolarSigmoid.__DefaultInstance));
        fnList.Add(new ActivationFunctionInfo(2, 0.3, Gaussian.__DefaultInstance));
        fnList.Add(new ActivationFunctionInfo(3, 0.3, Sine.__DefaultInstance));
        
        
        ActivationFunctionLib = new DefaultActivationFunctionLibrary(fnList);;
        NeatGenomeParameters neatGenomeParams = new NeatGenomeParameters();
        neatGenomeParams.InitialInterconnectionsProportion = 0.9;
        neatGenomeParams.AddConnectionMutationProbability = 0.9;
        neatGenomeParams.AddNodeMutationProbability = 0.9;
        neatGenomeParams.ConnectionWeightMutationProbability = 0.9;
        neatGenomeParams.DisjointExcessGenesRecombinedProbability = 0.9;
        neatGenomeParams.NodeAuxStateMutationProbability = 0.2;
        neatGenomeParams.FeedforwardOnly = true;
        
        CppnGenomeFactory = new CppnGenomeFactory(InputCount, OutputCount, ActivationFunctionLib, neatGenomeParams);
        GenomeList = CppnGenomeFactory.CreateGenomeList(PopulationSize, Generation);
        
    }

    
    public void CreateNewGeneration() {
        Debug.Log("Created new gen!");
        
        Generation++;
        List<NeatGenome> tempList = new List<NeatGenome>();
    
        foreach (var genome in GenomeList)
        {
            if (genome.EvaluationInfo.IsEvaluated)
                tempList.Add(genome);
        }

        int randInt1;
        int randInt2;
        do {
            randInt1 = Random.Range(0, GenomeList.Count - 1);
            randInt2 = Random.Range(0, GenomeList.Count - 1);
        } while (randInt1 == randInt2);

        if (tempList.Count == 0) {
            tempList.Add(GenomeList[randInt1]);
            tempList.Add(GenomeList[randInt2]);
        }
    
        if (tempList.Count != 2)
            Debug.Log("You must choose exactly two genomes!");
        else
        {
            for (int i = 0; i < SexualOffspringCount; i++)
                GenomeList[i] = tempList[0].CreateOffspring(tempList[1], Generation);

            for (int i = SexualOffspringCount; i < PopulationSize; i++)
                GenomeList[i] = tempList[i % 2].CreateOffspring(Generation);
    
            NewGenEvent?.Invoke();
        }
        
        
    }
    
    
    
    
}
