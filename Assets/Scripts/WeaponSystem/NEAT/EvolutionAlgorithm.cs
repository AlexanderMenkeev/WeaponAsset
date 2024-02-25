using System;
using System.Collections.Generic;
using System.Linq;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.HyperNeat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using UnityEngine;
using Random = UnityEngine.Random;


namespace WeaponSystem.NEAT {
    public class EvolutionAlgorithm : MonoBehaviour
    {
        public IList<NeatGenome> GenomeList;
        
        public event Action NewGenEvent;
        public NeatGenomeDecoder Decoder;
        public CppnGenomeFactory CppnGenomeFactory;
        private NetworkActivationScheme _activationScheme;
        private IActivationFunctionLibrary _activationFunctionLib;

        [field: SerializeField] public int PopulationSize { private set; get; }
        [field: SerializeField] public uint Generation { private set; get; }
        [field: SerializeField] public int InputCount { private set; get; }
        [field: SerializeField] public int OutputCount { private set; get; }
        [field: SerializeField] public int CloneOffspringCount { private set; get; }
        [field: SerializeField] public int SexualOffspringCount { private set; get; }
    
        public static EvolutionAlgorithm Instance;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        
            InitializeEvolutionAlgorithmParams();
            InitializeEvolutionAlgorithm();
        }
    
        private void InitializeEvolutionAlgorithmParams() {
            PopulationSize = 6;
            _activationScheme = NetworkActivationScheme.CreateAcyclicScheme();
            Generation = 0;
            InputCount = 3;
            OutputCount = 5;
            CloneOffspringCount = 2;
            SexualOffspringCount = PopulationSize - 2;
        }
        private void InitializeEvolutionAlgorithm() {
            Decoder = new NeatGenomeDecoder(_activationScheme);
        
            List<ActivationFunctionInfo> fnList = new List<ActivationFunctionInfo>(4);
            fnList.Add(new ActivationFunctionInfo(0, 0.1, Linear.__DefaultInstance));
            fnList.Add(new ActivationFunctionInfo(1, 0.3, BipolarSigmoid.__DefaultInstance));
            fnList.Add(new ActivationFunctionInfo(2, 0.3, Gaussian.__DefaultInstance));
            fnList.Add(new ActivationFunctionInfo(3, 0.3, Sine.__DefaultInstance));
        
            _activationFunctionLib = new DefaultActivationFunctionLibrary(fnList);;
            NeatGenomeParameters neatGenomeParams = new NeatGenomeParameters();
            neatGenomeParams.InitialInterconnectionsProportion = 0.8;
            neatGenomeParams.AddConnectionMutationProbability = 0.8;
            neatGenomeParams.AddNodeMutationProbability = 0.8;
            neatGenomeParams.ConnectionWeightMutationProbability = 0.8;
            neatGenomeParams.DisjointExcessGenesRecombinedProbability = 0.5;
            neatGenomeParams.NodeAuxStateMutationProbability = 0.2;
            neatGenomeParams.FeedforwardOnly = true;
        
            CppnGenomeFactory = new CppnGenomeFactory(InputCount, OutputCount, _activationFunctionLib, neatGenomeParams);
            GenomeList = CppnGenomeFactory.CreateGenomeList(PopulationSize, Generation);
        }

    
        public void CreateNewGeneration() {
            Debug.Log("Created new generation!");
        
            Generation++;
            List<NeatGenome> selectedGenomes = GenomeList.Where(genome => genome.EvaluationInfo.IsEvaluated).ToList();
            List<NeatGenome> rejectedGenomes = GenomeList.Where(genome => !genome.EvaluationInfo.IsEvaluated).ToList();
            
            // if no genomes were selected => select two randomly
            if (selectedGenomes.Count == 0) {
                int randInt1, randInt2;
                do {
                    randInt1 = Random.Range(0, GenomeList.Count);
                    randInt2 = Random.Range(0, GenomeList.Count);
                } while (randInt1 == randInt2);
                
                selectedGenomes.Add(GenomeList[randInt1]);
                selectedGenomes.Add(GenomeList[randInt2]);
            }
            
            int count = selectedGenomes.Count;
            // if one genome was selected => pair it up with rejected genomes for sexual reproduction
            if (count == 1) {
                for (int i = 0; i < SexualOffspringCount; i++) 
                    GenomeList[i] = rejectedGenomes[i].CreateOffspring(selectedGenomes[0], Generation);
            } 
            // if number of selected genomes is sufficient => breed them with each other
            else {
                for (int i = 0; i < SexualOffspringCount; i++) {
                    int indexA, indexB = i % count; 
                    do {
                        indexA = Random.Range(0, count);
                    } while (indexA == indexB);
                    
                    GenomeList[i] = selectedGenomes[indexA].CreateOffspring(selectedGenomes[indexB], Generation);
                }
            }
            
            // asexual reproduction is the same for both cases
            for (int i = SexualOffspringCount; i < PopulationSize; i++)
                GenomeList[i] = selectedGenomes[i % count].CreateOffspring(Generation);
            
            
            NewGenEvent?.Invoke();
        }
        
        public void CreateRandomPopulation() {
            Debug.Log("Created random population!");

            Generation = 0;
            CppnGenomeFactory.GenomeIdGenerator.Reset();
            for (int i = 0; i < PopulationSize; i++) {
                CppnGenomeFactory.InnovationIdGenerator.Reset();
                GenomeList[i] = CppnGenomeFactory.CreateGenome(Generation);
            }
            
            NewGenEvent?.Invoke();
        }
    
    
    
    
    }
}
