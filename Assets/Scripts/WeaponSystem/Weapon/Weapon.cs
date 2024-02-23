using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SharpNeat.Genomes.Neat;
using SODefinitions;
using UnityEngine;
using WeaponSystem.NEAT;

namespace WeaponSystem.Weapon {
    public class Weapon : AbstractWeapon
    {
        private void Awake() { base.OnAwakeFunc(); }

        private void OnDestroy() { base.OnDestroyFunc(); }
        

        private string GenerateHash() {
            return DateTime.Now.Ticks.GetHashCode().ToString("x").ToUpper();
        }
        
        private string _savePath;
        private string _hash;
        public void SaveFunc(bool generateName, string fileName) {
            if (!Application.isPlaying) {
                Debug.Log("Play mode only");
                return;
            }
            
            XmlWriterSettings xwSettings = new XmlWriterSettings {
                Indent = true
            };
            
            _hash = GenerateHash();
            _savePath = Path.Combine(Application.dataPath, "Resources\\ProjectileGenomes\\");
            _savePath += generateName ? _hash : fileName;
            Directory.CreateDirectory(_savePath);
            
            string genomeFileName = "Genome_" + _hash + ".xml";
            string paramsFileName = "Params_" + _hash + ".json";
            
            using(XmlWriter xw = XmlWriter.Create(Path.Combine(_savePath, genomeFileName), xwSettings)) {
                NeatGenomeXmlIO.WriteComplete(xw, GenomeStats.Genome, true);
            }
            
            string paramsJson = JsonUtility.ToJson(_weaponParamsLocal, true);
            using (FileStream fs = new FileStream(Path.Combine(_savePath, paramsFileName), FileMode.Create)){
                using (StreamWriter writer = new StreamWriter(fs)){
                    writer.Write(paramsJson);
                }
            }
            
            Debug.Log($"Genome and weapon params saved to path [{Path.Combine(_savePath, genomeFileName)}]");
            
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh ();
            #endif
        }

        
        public void LoadFunc(TextAsset genomeTextAsset) {
            if (!Application.isPlaying) {
                Debug.Log("Play mode only");
                return;
            }
            StopCoroutine(FireCoroutine);
            
            XmlDocument genomeXml = new XmlDocument();
            genomeXml.LoadXml(genomeTextAsset.text);
            
            List<NeatGenome> genomeList = NeatGenomeXmlIO.LoadCompleteGenomeList(genomeXml, true, EvolutionAlgorithm.Instance.CppnGenomeFactory);
            
            GenomeStats.UpdateGenomeStats(genomeList[0]);
            
            Debug.Log($"Genome {genomeTextAsset.name} is loaded");
        
            FireCoroutine = StartCoroutine(FireProjectile());
        }
        
        
        public void EvaluateGenome() {
            if (!Application.isPlaying) {
                Debug.Log("Play mode only");
                return;
            }
                
            GenomeStats.Genome.EvaluationInfo.SetFitness(10);
        }
        
        
        private void Start() {
            base.OnStartFunc();
        }

        public void UpdateWeaponSO(WeaponParamsSO weaponSo) {
            StopCoroutine(FireCoroutine);
            
            _weaponSO.DestroyProjectilesEvent?.Invoke();
            _weaponSO = weaponSo;
            base.InitializeParams();

            FireCoroutine = StartCoroutine(FireProjectile());
        }
        
        
   
    }
}
