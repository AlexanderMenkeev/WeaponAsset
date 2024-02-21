using SODefinitions;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

namespace Editor {
    [CustomEditor(typeof(WeaponParamsSO))]
    public class WeaponParamsEditor : UnityEditor.Editor {
        public float SomeValue;

        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            // exact positioning
            // GUI
            // EditorGUI
            
            GUILayout.Label("Noot");
            GUILayout.Button("SomeButton");

            GUILayout.HorizontalSlider(SomeValue, 1f, 3f);
            // implicit positioning, auto-layout
            // GUILayout
            // EditorGUILayout


        }

    }
}
