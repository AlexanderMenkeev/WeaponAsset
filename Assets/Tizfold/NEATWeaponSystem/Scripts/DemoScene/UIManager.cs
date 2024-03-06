using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tizfold.NEATWeaponSystem.Scripts.DemoScene {
    public class UIManager : MonoBehaviour {
        
        // assigned from the editor
        [SerializeField] private Button _prevBtn;
        [SerializeField] private Button _nextBtn;
        [SerializeField] private TextMeshProUGUI _weaponText;
        [SerializeField] private DemoWeapon _demoWeapon;
        [SerializeField] private WeaponParamsSO[] _SObjectsList;
        
        
        private int _counter;
        private int _weaponCount;
        private void Awake() {
            _counter = 0;
            _weaponCount = _SObjectsList.Length;

            foreach (WeaponParamsSO so in _SObjectsList) {
                so.LoadParamsFromJson();
            }
            
            _nextBtn.onClick.AddListener(OnNextBtnClick);
            _prevBtn.onClick.AddListener(OnPrevBtnClick);
        }

        private void OnDestroy() {
            _nextBtn.onClick.RemoveListener(OnNextBtnClick);
            _prevBtn.onClick.RemoveListener(OnPrevBtnClick);
        }

        private void Start() {
            _weaponText.text = $"{_SObjectsList[_counter].name} #{_counter}";
            _demoWeapon.UpdateWeaponSO(_SObjectsList[_counter]);
        }

        private void OnNextBtnClick() {
            if (_counter == _weaponCount - 1)
                _counter = 0;
            else
                _counter++;

            _weaponText.text = $"{_SObjectsList[_counter].name} #{_counter}";
            
            _demoWeapon.UpdateWeaponSO(_SObjectsList[_counter]);
        }
        
        private void OnPrevBtnClick() {
            if (_counter == 0)
                _counter = _weaponCount - 1;
            else
                _counter--;
            
            _weaponText.text = $"{_SObjectsList[_counter].name} #{_counter}";
            
            _demoWeapon.UpdateWeaponSO(_SObjectsList[_counter]);
        }
        
        



    }
}
