using System;
using SODefinitions;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using WeaponSystem.Weapon;
using Object = UnityEngine.Object;

namespace Editor {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Weapon))]
    public class WeaponEditor : UnityEditor.Editor {
        
        private TextAsset _genomeTextAsset;
        private string _genomeFileName;
        private bool _generateUniqueName;
        private bool _selectedAsParent;

        private void OnEnable() {
            _genomeTextAsset = null;
            _genomeFileName = "File name";
            _generateUniqueName = true;
            _selectedAsParent = false;
        }

        public Object source;

        public override void OnInspectorGUI() {
            
            
            base.OnInspectorGUI();
            
            GUILayout.Space(10);
            
            Weapon weapon = target as Weapon;
            if (weapon == null)
                return;
            _selectedAsParent = weapon.GenomeStats.IsEvaluated;
            
            using(new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUILayout.Space(5);
                
                GUILayout.Label("Genome controls", EditorStyles.boldLabel);
                
                GUILayout.Space(3);
                
                
                if (!_selectedAsParent) {
                    if (GUILayout.Button("Select as parent for next gen")) {
                        weapon.EvaluateGenome();
                        _selectedAsParent = true;
                        weapon.GenomeStats.IsEvaluated = true;
                    }
                }
                else {
                    GUI.enabled = false;
                    GUILayout.Button("Already selected");
                    GUI.enabled = true;
                }

                GUILayout.Space(2);
                
                _generateUniqueName = EditorGUILayout.ToggleLeft("Generate unique hash name", _generateUniqueName, GUILayout.ExpandWidth(false));
                
                using(new GUILayout.HorizontalScope()) {
                    if (GUILayout.Button("Save") )
                        weapon.SaveFunc(_generateUniqueName, _genomeFileName);
                    if (!_generateUniqueName)
                        _genomeFileName = EditorGUILayout.TextField(_genomeFileName, GUILayout.MaxWidth(300));
                }
                
                GUILayout.Space(2);
                
                using(new GUILayout.HorizontalScope()) {
                    if (_genomeTextAsset == null)
                        GUI.enabled = false;
                    if ( GUILayout.Button("Load") )
                        weapon.LoadFunc(_genomeTextAsset);
                    GUI.enabled = true;
                    
                    _genomeTextAsset = EditorGUILayout.ObjectField(_genomeTextAsset, typeof(TextAsset), false, GUILayout.MaxWidth(300)) as TextAsset;
                }
                
                GUILayout.Space(10);
            }
            
            
            
            
           


        }

    }
}

