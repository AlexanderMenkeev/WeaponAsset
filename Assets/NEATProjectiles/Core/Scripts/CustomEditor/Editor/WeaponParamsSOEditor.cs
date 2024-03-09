using System;
using NEATProjectiles.Core.Scripts.SODefinitions;
using NEATProjectiles.Core.Scripts.WeaponSystem;
using UnityEditor;
using UnityEngine;

namespace NEATProjectiles.Core.Scripts.CustomEditor.Editor {
    [CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(WeaponParamsSO))]
    public class WeaponParamsSOEditor : UnityEditor.Editor {
        
        private SerializedObject _so;

        private SerializedProperty GenomeXml;
        private SerializedProperty WeaponParamsJson;
        
        private SerializedProperty WeaponMode;
        private SerializedProperty BurstMode;
        private SerializedProperty BurstRate;
        
        private SerializedProperty FireRate;
        private SerializedProperty ProjectilesInOneShot;
        
        private SerializedProperty RotationSpeed;
        private SerializedProperty MoveSpeed;
        
        private SerializedProperty PositioningMode;
        private SerializedProperty ReadMode;
        private SerializedProperty Size;
        private SerializedProperty Lifespan;
        
        private SerializedProperty HueRange;
        private SerializedProperty Saturation;
        private SerializedProperty Brightness;
        
        private SerializedProperty SpeedRange;
        private SerializedProperty ForceRange;
        private SerializedProperty NNControlDistance;
        private SerializedProperty SignX;
        private SerializedProperty SignY;
        private SerializedProperty ForwardForce;
        
        private SerializedProperty InitialFlightRadius;
        private SerializedProperty InitialSpeed;
        private SerializedProperty Angle;
        
        private SerializedProperty FlipXOnReflect;
        private SerializedProperty FlipYOnReflect;
        private SerializedProperty Mode;
        
        private SerializedProperty ReflectiveCircleRadius;
        private SerializedProperty RectDimensions;
        private SerializedProperty MaxPolarAngleDeg;
        
        
        private void OnEnable() {
            _so = serializedObject;

            GenomeXml = _so.FindProperty(nameof(WeaponParamsSO.GenomeXml));
            WeaponParamsJson = _so.FindProperty(nameof(WeaponParamsSO.WeaponParamsJson));
            
            WeaponMode = _so.FindProperty($"<{nameof(WeaponParamsSO.WeaponMode)}>k__BackingField");
            BurstMode = _so.FindProperty($"<{nameof(WeaponParamsSO.BurstMode)}>k__BackingField");
            BurstRate = _so.FindProperty($"<{nameof(WeaponParamsSO.BurstRate)}>k__BackingField");
            
            FireRate = _so.FindProperty($"<{nameof(WeaponParamsSO.FireRate)}>k__BackingField");
            ProjectilesInOneShot = _so.FindProperty($"<{nameof(WeaponParamsSO.ProjectilesInOneShot)}>k__BackingField");
            
            RotationSpeed = _so.FindProperty($"<{nameof(WeaponParamsSO.RotationSpeed)}>k__BackingField");
            MoveSpeed = _so.FindProperty($"<{nameof(WeaponParamsSO.MoveSpeed)}>k__BackingField");
            
            PositioningMode = _so.FindProperty($"<{nameof(WeaponParamsSO.PositioningMode)}>k__BackingField");
            ReadMode = _so.FindProperty($"<{nameof(WeaponParamsSO.ReadMode)}>k__BackingField");
            Size = _so.FindProperty($"<{nameof(WeaponParamsSO.Size)}>k__BackingField");
            Lifespan = _so.FindProperty($"<{nameof(WeaponParamsSO.Lifespan)}>k__BackingField");
            
            HueRange = _so.FindProperty($"<{nameof(WeaponParamsSO.HueRange)}>k__BackingField");
            Saturation = _so.FindProperty($"<{nameof(WeaponParamsSO.Saturation)}>k__BackingField");
            Brightness = _so.FindProperty($"<{nameof(WeaponParamsSO.Brightness)}>k__BackingField");
            
            SpeedRange = _so.FindProperty($"<{nameof(WeaponParamsSO.SpeedRange)}>k__BackingField");
            ForceRange = _so.FindProperty($"<{nameof(WeaponParamsSO.ForceRange)}>k__BackingField");
            NNControlDistance = _so.FindProperty($"<{nameof(WeaponParamsSO.NNControlDistance)}>k__BackingField");
            SignX = _so.FindProperty($"<{nameof(WeaponParamsSO.SignX)}>k__BackingField");
            SignY = _so.FindProperty($"<{nameof(WeaponParamsSO.SignY)}>k__BackingField");
            ForwardForce = _so.FindProperty($"<{nameof(WeaponParamsSO.ForwardForce)}>k__BackingField");
            
            InitialFlightRadius = _so.FindProperty($"<{nameof(WeaponParamsSO.InitialFlightRadius)}>k__BackingField");
            InitialSpeed = _so.FindProperty($"<{nameof(WeaponParamsSO.InitialSpeed)}>k__BackingField");
            Angle = _so.FindProperty($"<{nameof(WeaponParamsSO.Angle)}>k__BackingField");
            
            Mode = _so.FindProperty($"<{nameof(WeaponParamsSO.Mode)}>k__BackingField");
            FlipXOnReflect = _so.FindProperty($"<{nameof(WeaponParamsSO.FlipXOnReflect)}>k__BackingField");
            FlipYOnReflect = _so.FindProperty($"<{nameof(WeaponParamsSO.FlipYOnReflect)}>k__BackingField");
            
            ReflectiveCircleRadius = _so.FindProperty($"<{nameof(WeaponParamsSO.ReflectiveCircleRadius)}>k__BackingField");
            RectDimensions = _so.FindProperty($"<{nameof(WeaponParamsSO.RectDimensions)}>k__BackingField");
            MaxPolarAngleDeg = _so.FindProperty($"<{nameof(WeaponParamsSO.MaxPolarAngleDeg)}>k__BackingField");
        }
        
        private bool _rename = true;
        public override void OnInspectorGUI() {
            
            _so.Update();
            
            WeaponParamsSO weaponSO = target as WeaponParamsSO;
            if (weaponSO == null) {
                Debug.LogException(new Exception("Target is null!"));
                return;
            }
            
            WeaponParamsSO[] wParamsArray = Array.ConvertAll(_so.targetObjects, item => (WeaponParamsSO)item);
            if (wParamsArray.Length == 0) {
                Debug.LogException(new Exception("Zero targets!"));
                return;
            }
            
            GUILayout.Space(8);
            
            if ( GUILayout.Button("Reset to default values") )
                foreach (WeaponParamsSO wp in wParamsArray) {
                    wp.ResetFunc();
                }
            
            GUILayout.Space(8);
            
            GUILayout.Label("Load files", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUILayout.Space(2);
                using(new GUILayout.HorizontalScope()) {
                    
                    if (GUILayout.Button("Load files from folder", GUILayout.MaxWidth(EditorGUIUtility.labelWidth))) {
                        foreach (WeaponParamsSO wp in wParamsArray) {
                            wp.LoadGenomeAndParamsFromFolder(_rename);
                        }
                        _so.Update();
                        _so.ApplyModifiedProperties();
                        return;
                    }
                    _rename = EditorGUILayout.ToggleLeft("Rename", _rename, GUILayout.ExpandWidth(false));
                }
            
                GUILayout.Space(5);
                
                EditorGUILayout.PropertyField(GenomeXml);
                using(new GUILayout.HorizontalScope()) {
                    EditorGUILayout.PropertyField(WeaponParamsJson);
                    
                    if (wParamsArray[0].WeaponParamsJson == null)
                        GUI.enabled = false;
                    if ( GUILayout.Button("Load") )
                        foreach (WeaponParamsSO wp in wParamsArray) {
                            wp.LoadParamsFromJson();
                        }
                    GUI.enabled = true;
                }
                
                GUILayout.Space(3);
            }
            
            GUILayout.Space(15);
            
            GUILayout.Label("Weapon", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(FireRate);
                EditorGUILayout.PropertyField(ProjectilesInOneShot);
                EditorGUILayout.PropertyField(WeaponMode);
                if (weaponSO.WeaponMode == global::NEATProjectiles.Core.Scripts.WeaponSystem.WeaponMode.Burst) {
                    using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(BurstMode);
                        EditorGUILayout.PropertyField(BurstRate);
                        GUILayout.Space(3);
                    }
                }
                
                GUILayout.Space(10);
                
                if ( GUILayout.Button("Destroy projectiles") )
                    foreach (WeaponParamsSO wp in wParamsArray) {
                        wp.DestroyProjectilesEvent?.Invoke();
                    }
                
                GUILayout.Space(3);
            }
            
            GUILayout.Space(15);
            
            GUILayout.Label("Coordinate System", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {

                GUILayout.Space(2);
                EditorGUILayout.PropertyField(RotationSpeed);
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(MoveSpeed);
                GUILayout.Space(10);
                
                if ( GUILayout.Button("Apply for all") )
                    foreach (WeaponParamsSO wp in wParamsArray) {
                        wp.ApplyForAllCoordinateSystemsEvent?.Invoke();
                    }
                
                GUILayout.Space(3);
            }
            
            
            GUILayout.Space(15);
            
            GUILayout.Label("Projectile", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(PositioningMode);
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(ReadMode);
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(Size);
                EditorGUILayout.PropertyField(Lifespan);
                GUILayout.Space(5);
            
                GUILayout.Label("Color", EditorStyles.miniBoldLabel);
                using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                    GUILayout.Space(2);
                    EditorGUILayout.PropertyField(HueRange);
                    EditorGUILayout.PropertyField(Saturation);
                    EditorGUILayout.PropertyField(Brightness);
                    GUILayout.Space(3);
                }
                GUILayout.Space(3);
            }
            
            GUILayout.Space(15);
            
            GUILayout.Label("Neural Network Pattern", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(SpeedRange);
                EditorGUILayout.PropertyField(ForceRange);
                EditorGUILayout.PropertyField(NNControlDistance);
                EditorGUILayout.PropertyField(SignX);
                EditorGUILayout.PropertyField(SignY);
                EditorGUILayout.PropertyField(ForwardForce);
                GUILayout.Space(3);
                
            }
            
            GUILayout.Space(15);
            
            GUILayout.Label("Initial Flight", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(InitialFlightRadius);
                EditorGUILayout.PropertyField(InitialSpeed);
                EditorGUILayout.PropertyField(Angle);
                GUILayout.Space(3);
                
            }
            
            GUILayout.Space(15);
            
            GUILayout.Label("Reflection", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                
                GUILayout.Space(2);
                
                EditorGUILayout.PropertyField(FlipXOnReflect);
                EditorGUILayout.PropertyField(FlipYOnReflect);
                EditorGUILayout.PropertyField(Mode);
                
                switch (weaponSO.Mode) {
            
                    case ReflectionMode.CircleReflection:
                        EditorGUILayout.PropertyField(ReflectiveCircleRadius);
                        break;
                    
                    case ReflectionMode.RectangleReflection:
                        EditorGUILayout.PropertyField(RectDimensions);
                        break;
                    
                    case ReflectionMode.Polar:
                        EditorGUILayout.PropertyField(MaxPolarAngleDeg);
                        if (weaponSO.Angle > weaponSO.MaxPolarAngleDeg)
                            Debug.LogWarning("Angle of Initial Flight is bigger than Max Polar Angle!");
                            
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                GUILayout.Space(3);
                
            }
            
            
            _so.ApplyModifiedProperties();
        }

        
        
        
        
    }
}

