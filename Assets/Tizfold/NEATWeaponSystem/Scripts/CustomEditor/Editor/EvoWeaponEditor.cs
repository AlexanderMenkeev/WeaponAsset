using Tizfold.NEATWeaponSystem.Scripts.EvolutionScene;
using UnityEditor;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.CustomEditor.Editor {
    [CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(EvoWeapon))]
    public class EvoWeaponEditor : UnityEditor.Editor {
        
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
        
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            
            GUILayout.Space(10);
            
            EvoWeapon weapon = target as EvoWeapon;
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

