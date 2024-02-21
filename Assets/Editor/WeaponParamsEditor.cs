using System;
using SODefinitions;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

namespace Editor {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WeaponParamsSO))]
    public class WeaponParamsEditor : UnityEditor.Editor {
        
        private SerializedObject _so;

        private SerializedProperty FireRate;
        private SerializedProperty ProjectilesInOneShot;
        private SerializedProperty Size;
        private SerializedProperty Lifespan;
        private SerializedProperty HueRange;
        private SerializedProperty Saturation;
        private SerializedProperty Brightness;
        
        private SerializedProperty SpeedRange;
        private SerializedProperty ForceRange;
        
        private SerializedProperty NNControlDistance;
        private SerializedProperty MaxPolarAngleDeg;
        private SerializedProperty FlipY;
        
        private SerializedProperty ReflectiveCircleRadius;
        
        private SerializedProperty InitialFlightRadius;
        private SerializedProperty InitialSpeed;
        private SerializedProperty Angle;
        
        private SerializedProperty RectDimensions;
        
        private SerializedProperty FlipXOnReflect;
        private SerializedProperty FlipYOnReflect;
        private SerializedProperty ForwardForce;
        
        private SerializedProperty Mode;
        
        
        private void OnEnable() {
            _so = serializedObject;
            FireRate = _so.FindProperty($"<{nameof(WeaponParamsSO.FireRate)}>k__BackingField");
            ProjectilesInOneShot = _so.FindProperty($"<{nameof(WeaponParamsSO.ProjectilesInOneShot)}>k__BackingField");
            Size = _so.FindProperty($"<{nameof(WeaponParamsSO.Size)}>k__BackingField");
            Lifespan = _so.FindProperty($"<{nameof(WeaponParamsSO.Lifespan)}>k__BackingField");
            HueRange = _so.FindProperty($"<{nameof(WeaponParamsSO.HueRange)}>k__BackingField");
            Saturation = _so.FindProperty($"<{nameof(WeaponParamsSO.Saturation)}>k__BackingField");
            Brightness = _so.FindProperty($"<{nameof(WeaponParamsSO.Brightness)}>k__BackingField");
            
            SpeedRange = _so.FindProperty($"<{nameof(WeaponParamsSO.SpeedRange)}>k__BackingField");
            ForceRange = _so.FindProperty($"<{nameof(WeaponParamsSO.ForceRange)}>k__BackingField");
            
            NNControlDistance = _so.FindProperty($"<{nameof(WeaponParamsSO.NNControlDistance)}>k__BackingField");
            MaxPolarAngleDeg = _so.FindProperty($"<{nameof(WeaponParamsSO.MaxPolarAngleDeg)}>k__BackingField");
            FlipY = _so.FindProperty($"<{nameof(WeaponParamsSO.FlipY)}>k__BackingField");
            
            ReflectiveCircleRadius = _so.FindProperty($"<{nameof(WeaponParamsSO.ReflectiveCircleRadius)}>k__BackingField");
            
            InitialFlightRadius = _so.FindProperty($"<{nameof(WeaponParamsSO.InitialFlightRadius)}>k__BackingField");
            InitialSpeed = _so.FindProperty($"<{nameof(WeaponParamsSO.InitialSpeed)}>k__BackingField");
            Angle = _so.FindProperty($"<{nameof(WeaponParamsSO.Angle)}>k__BackingField");
            
            RectDimensions = _so.FindProperty($"<{nameof(WeaponParamsSO.RectDimensions)}>k__BackingField");
            
            FlipXOnReflect = _so.FindProperty($"<{nameof(WeaponParamsSO.FlipXOnReflect)}>k__BackingField");
            FlipYOnReflect = _so.FindProperty($"<{nameof(WeaponParamsSO.FlipYOnReflect)}>k__BackingField");
            ForwardForce = _so.FindProperty($"<{nameof(WeaponParamsSO.ForwardForce)}>k__BackingField");
            
            Mode = _so.FindProperty($"<{nameof(WeaponParamsSO.Mode)}>k__BackingField");
        }


        public override void OnInspectorGUI() {
            
            _so.Update();
            
            GUILayout.Label("Weapon", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(FireRate);
                EditorGUILayout.PropertyField(ProjectilesInOneShot);
                GUILayout.Space(3);
            }
            
            GUILayout.Space(15);
            
            GUILayout.Label("Projectile", EditorStyles.boldLabel);
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                
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
                
                WeaponParamsSO weaponSO = target as WeaponParamsSO;
                if (weaponSO == null) {
                    Debug.LogException(new Exception("WeaponSO is null!"));
                    return;
                }
                
                switch (weaponSO.Mode) {

                    case ProjectileMode.CircleReflection:
                        EditorGUILayout.PropertyField(ReflectiveCircleRadius);
                        break;
                    
                    case ProjectileMode.RectangleReflection:
                        EditorGUILayout.PropertyField(RectDimensions);
                        break;
                    
                    case ProjectileMode.Polar:
                        EditorGUILayout.PropertyField(MaxPolarAngleDeg);
                        if (weaponSO.Angle > weaponSO.MaxPolarAngleDeg)
                            Debug.Log("Angle in Initial Flight is bigger than Max Polar Angle!");
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

