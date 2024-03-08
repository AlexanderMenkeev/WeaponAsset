using System;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using UnityEditor;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.CustomEditor.Editor {
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
        private SerializedProperty LaunchSpeed;
        
        private SerializedProperty PositioningMode;
        private SerializedProperty Size;
        private SerializedProperty Lifespan;
        
        private SerializedProperty HueRange;
        private SerializedProperty Saturation;
        private SerializedProperty Brightness;
        
        private SerializedProperty SpeedRange;
        private SerializedProperty ForceRange;
        private SerializedProperty NNControlDistance;
        private SerializedProperty FlipX;
        private SerializedProperty FlipY;
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
            LaunchSpeed = _so.FindProperty($"<{nameof(WeaponParamsSO.LaunchSpeed)}>k__BackingField");
            
            PositioningMode = _so.FindProperty($"<{nameof(WeaponParamsSO.PositioningMode)}>k__BackingField");
            Size = _so.FindProperty($"<{nameof(WeaponParamsSO.Size)}>k__BackingField");
            Lifespan = _so.FindProperty($"<{nameof(WeaponParamsSO.Lifespan)}>k__BackingField");
            
            HueRange = _so.FindProperty($"<{nameof(WeaponParamsSO.HueRange)}>k__BackingField");
            Saturation = _so.FindProperty($"<{nameof(WeaponParamsSO.Saturation)}>k__BackingField");
            Brightness = _so.FindProperty($"<{nameof(WeaponParamsSO.Brightness)}>k__BackingField");
            
            SpeedRange = _so.FindProperty($"<{nameof(WeaponParamsSO.SpeedRange)}>k__BackingField");
            ForceRange = _so.FindProperty($"<{nameof(WeaponParamsSO.ForceRange)}>k__BackingField");
            NNControlDistance = _so.FindProperty($"<{nameof(WeaponParamsSO.NNControlDistance)}>k__BackingField");
            FlipX = _so.FindProperty($"<{nameof(WeaponParamsSO.FlipX)}>k__BackingField");
            FlipY = _so.FindProperty($"<{nameof(WeaponParamsSO.FlipY)}>k__BackingField");
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
                if (weaponSO.WeaponMode == global::WeaponMode.Burst) {
                    using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(BurstMode);
                        EditorGUILayout.PropertyField(BurstRate);
                        GUILayout.Space(3);
                    }
                }
                
                GUILayout.Space(10);
                
                using(new GUILayout.HorizontalScope()) {
                    if ( GUILayout.Button("Launch projectiles forward", GUILayout.MaxWidth(EditorGUIUtility.labelWidth)) )
                        foreach (WeaponParamsSO wp in wParamsArray) {
                            wp.LaunchForwardEvent?.Invoke(wp.LaunchSpeed, Vector3.right);
                        }
                    
                    using( new GUILayout.HorizontalScope() ) {
                        EditorGUILayout.LabelField("Speed", GUILayout.Width(50));
                        EditorGUILayout.PropertyField(LaunchSpeed, label: GUIContent.none);
                    }
                }
                
                GUILayout.Space(5);
                
                if ( GUILayout.Button("Destroy projectiles") )
                    foreach (WeaponParamsSO wp in wParamsArray) {
                        wp.DestroyProjectilesEvent?.Invoke();
                    }
                
                GUILayout.Space(3);
            }
            
            GUILayout.Space(15);
            
            GUILayout.Label("Projectile", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(PositioningMode);
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
                EditorGUILayout.PropertyField(FlipX);
                EditorGUILayout.PropertyField(FlipY);
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

