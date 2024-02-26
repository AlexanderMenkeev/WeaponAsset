using System;
using System.IO;
using Interfaces;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using WeaponSystem.NEAT;
using WeaponSystem.Weapon;

namespace SODefinitions {
    [CreateAssetMenu(menuName = "ScriptableObjects/WeaponParamsSO", fileName = "WP_new")]
    public class WeaponParamsSO : ScriptableObject, IWeaponParams {
        
        public TextAsset GenomeXml;
        public TextAsset WeaponParamsJson;
        
        [field: SerializeField] [field: Range(0.3f, 1f)] public float FireRate { get; set; }
        [field: SerializeField] [field: Range(1, 20)] public int ProjectilesInOneShot { get; set; }
        
        [field: SerializeField] public Vector2 Size { get; set; }
        [field: SerializeField] [field: Range(2f, 10f)] public float Lifespan { get; set; }
        
        [field: SerializeField] [field: MinMaxRangeAttribute.MinMaxRange(0f, 1f, 2)] public Vector2 HueRange { get; set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float Saturation { get; set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float Brightness { get; set; }
        
        [field: SerializeField] [field: MinMaxRangeAttribute.MinMaxRange(1f, 8f)] public Vector2 SpeedRange { get; set; }
        [field: SerializeField] [field: MinMaxRangeAttribute.MinMaxRange(0.5f, 5f)] public Vector2 ForceRange { get; set; }
        [field: SerializeField] [field: Range(1f, 8f)] public float NNControlDistance { get; set; }
        [field: SerializeField] public bool FlipY { get; set; }
        [field: SerializeField] public bool ForwardForce { get; set; }
        
        [field: SerializeField] [field:Range(0.05f, 0.4f)] public float InitialFlightRadius { get; set; }
        [field: SerializeField] [field:Range(0.5f, 5f)] public float InitialSpeed { get; set; }
        [field: SerializeField] [field:Range(1f, 179f)] public float Angle { get; set; }
        
        [field: SerializeField] public bool FlipXOnReflect { get; set; }
        [field: SerializeField] public bool FlipYOnReflect { get; set; }
        [field: SerializeField] public ProjectileMode Mode { get; set; }
        
        [field: SerializeField] [field:Range(math.SQRT2, 2f)] public float ReflectiveCircleRadius { get; set; }
        [field: SerializeField] public Vector2 RectDimensions { get; set; }
        [field: SerializeField] [field: Range(5f, 179.9f)] public float MaxPolarAngleDeg { get; set; }
        
        
        public delegate void DestroyProjectiles();
        public DestroyProjectiles DestroyProjectilesEvent;
        
        public delegate void UpdateDelegate();
        public UpdateDelegate UpdateParamsEvent;
        
        // Different set of params
        private void InitializeParams() {
            FireRate = 1f;
            ProjectilesInOneShot = 10;
            
            Size = new Vector2(0.03f, 0.18f);
            Lifespan = 6f;
            HueRange = new Vector2(0.1f, 1f);
            Saturation = 0.9f;
            Brightness = 0.95f;
            
            SpeedRange = new Vector2(3f, 6f);
            ForceRange = new Vector2(1f, 3f);
            NNControlDistance = 3f;
            FlipY = false;
            ForwardForce = false;
            
            InitialFlightRadius = 0.1f;
            InitialSpeed = 2f;
            Angle = 40;

            FlipXOnReflect = true;
            FlipYOnReflect = true;
            Mode = ProjectileMode.CircleReflection;
            ReflectiveCircleRadius = math.SQRT2;
            RectDimensions = new Vector2(1f, 2f);
            MaxPolarAngleDeg = 45f;
        }

        // Default params
        private void InitializeParamsParticles() {
            FireRate = 1f;
            ProjectilesInOneShot = 20;
            
            Size = new Vector2(0.07f, 0.07f);
            Lifespan = 8f;
            HueRange = new Vector2(0.1f, 0.5f);
            Saturation = 0.9f;
            Brightness = 0.95f;
            
            SpeedRange = new Vector2(3f, 6f);
            ForceRange = new Vector2(1f, 3f);
            NNControlDistance = 3f;
            FlipY = false;
            ForwardForce = false;
            
            InitialFlightRadius = 0.1f;
            InitialSpeed = 2f;
            Angle = 70;

            FlipXOnReflect = true;
            FlipYOnReflect = true;
            Mode = ProjectileMode.CircleReflection;
            ReflectiveCircleRadius = math.SQRT2;
            RectDimensions = new Vector2(1f, 2f);
            MaxPolarAngleDeg = 45f;
        }
        
        public void ResetFunc() {
            InitializeParamsParticles();
            UpdateParamsEvent?.Invoke();
        }

        private void OnValidate() {
            UpdateParamsEvent?.Invoke();
        }

        public void LoadParamsFromJson() {
            if (WeaponParamsJson == null){
                Debug.LogWarning("Could not load. Json is null.");
                return;
            }
        
            WeaponParams weaponParams = JsonUtility.FromJson(WeaponParamsJson.text, typeof(WeaponParams)) as WeaponParams;
            WeaponParams.Copy(weaponParams, this);
            UpdateParamsEvent?.Invoke();
        }

        
        
        public void LoadGenomeAndParamsFromFolder(bool rename = false) {
            string folderName = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            string[] genomeGuid = AssetDatabase.FindAssets("Genome_ t:Object", new[] {folderName});
            string[] paramsGuid = AssetDatabase.FindAssets("Params_ t:Object", new[] {folderName});

            
            if (genomeGuid.Length != 1 || paramsGuid.Length != 1) {
                Debug.LogWarning($"Could not load. There must be exactly one genome_file and one params_file in {folderName}.");
                return;
            }
            
            GenomeXml = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(genomeGuid[0]));
            WeaponParamsJson = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(paramsGuid[0]));

            if (rename) {
                string fileName = "WP_" + GenomeXml.name.Split("_")[1];
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), fileName);
            }
            LoadParamsFromJson();
        }
        
        

        
        
    }
    
    
    public enum ProjectileMode {
        CircleReflection,
        RectangleReflection,
        Polar
    }
}
