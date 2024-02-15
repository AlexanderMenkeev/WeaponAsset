using System.Collections.Generic;
using System.Xml;
using SharpNeat.Genomes.Neat;
using UnityEngine;

namespace WeaponSystem {
    public class GameSceneWeapon : AbstractWeapon
    {
        private void Awake()
        {
            TemporalObjects = GameObject.Find("TemporalObjects");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
            InitializeParams();
            _weaponParamsGlobal.UpdateParamsEvent += InitializeParams;
        }
    
        private void OnDestroy()
        {
            _weaponParamsGlobal.UpdateParamsEvent -= InitializeParams;
        }
    
        private void Start()
        {
            Factory = EvolutionAlgorithm.Instance.CppnGenomeFactory;
            Decoder = EvolutionAlgorithm.Instance.Decoder;
        
            TextAsset textAsset = (TextAsset) Resources.Load("BestGenome");  
            XmlDocument genomeXml = new XmlDocument();
            genomeXml.LoadXml(textAsset.text);
        
            List<NeatGenome> genomeList = NeatGenomeXmlIO.LoadCompleteGenomeList(genomeXml, true, Factory);
            ProjectileGenome = genomeList[0];
        }

    
    }
}