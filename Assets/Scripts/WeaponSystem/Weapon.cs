using System.Collections.Generic;
using System.IO;
using System.Xml;
using SharpNeat.Genomes.Neat;
using UnityEngine;

namespace WeaponSystem {
    public class Weapon : AbstractWeapon
    {
        private void Awake() {
            TemporalObjects = GameObject.Find("TemporalObjects");
            ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
            InitializeParams();
            _weaponParams.UpdateParamsEvent += InitializeParams;
        }

        private void OnDestroy() {
            _weaponParams.UpdateParamsEvent -= InitializeParams;
        }

        private void OnValidate()
        {
            if(!Application.isPlaying) return;
            if (IsEvaluated)
                ProjectileGenome.EvaluationInfo.SetFitness(10);
        
            if (Save) {
                XmlWriterSettings xwSettings = new XmlWriterSettings();
                xwSettings.Indent = true;

                string resourcesPath = Path.Combine(Application.dataPath, "Resources");
            
                using(XmlWriter xw = XmlWriter.Create(Path.Combine(resourcesPath, "BestGenome.xml"), xwSettings)) {
                    NeatGenomeXmlIO.WriteComplete(xw, new NeatGenome[] { ProjectileGenome }, true);
                }
                Debug.Log($"Best genome saved to path [{Path.Combine(resourcesPath, "BestGenome.xml")}]");
                Save = false;
            }

            if (ChangeOnBestGenome) {
                StopCoroutine(FireCoroutine);
            
                string resourcesPath = Path.Combine(Application.dataPath, "Resources");
            
                XmlDocument genomeXml = new XmlDocument();
                genomeXml.Load(Path.Combine(resourcesPath, "BestGenome.xml"));

                List<NeatGenome> genomeList = NeatGenomeXmlIO.LoadCompleteGenomeList(genomeXml, true, Factory);
                ProjectileGenome = genomeList[0];
                Debug.Log($"Best genome loaded from [{Path.Combine(resourcesPath, "BestGenome.xml")}]");
            
                FireCoroutine = StartCoroutine(FireProjectile());
                ChangeOnBestGenome = false;
            }
        }
    
   

        private void Start()
        {
            FireCoroutine = StartCoroutine(FireProjectile());
        }
        
        

   
    }
}
