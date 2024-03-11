using NEATProjectiles.Core.Scripts.SODefinitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NEATProjectiles.Demos.Scripts.DemoScene {
    public class UIBox : MonoBehaviour {
        
        // assigned from the editor
        [SerializeField] private Button _prevBtn;
        [SerializeField] private Button _nextBtn;
        [SerializeField] private Button _launchBtn;
        [SerializeField] private TextMeshProUGUI _weaponText;
        [SerializeField] private DemoWeapon _demoWeapon;
        [SerializeField] private WeaponParamsSO[] _weaponList;
        
        
        private int _counter;
        private int _weaponCount;
        private void Awake() {
            _counter = 0;
            _weaponCount = _weaponList.Length;

            foreach (WeaponParamsSO so in _weaponList) {
                so.LoadParamsFromJson();
            }
            
            _nextBtn.onClick.AddListener(OnNextBtnClick);
            _prevBtn.onClick.AddListener(OnPrevBtnClick);
            _launchBtn.onClick.AddListener(OnLaunchClick);
        }

        private void OnDestroy() {
            _nextBtn.onClick.RemoveListener(OnNextBtnClick);
            _prevBtn.onClick.RemoveListener(OnPrevBtnClick);
            _launchBtn.onClick.RemoveListener(OnLaunchClick);
        }

        private void Start() {
            _weaponText.text = $"{_weaponList[_counter].name}\n#{_counter}";
            _demoWeapon.UpdateWeaponSO(_weaponList[_counter]);
        }

        private void OnNextBtnClick() {
            if (_counter == _weaponCount - 1)
                _counter = 0;
            else
                _counter++;

            _weaponText.text = $"{_weaponList[_counter].name}\n#{_counter}";
            
            _demoWeapon.UpdateWeaponSO(_weaponList[_counter]);
        }
        
        private void OnPrevBtnClick() {
            if (_counter == 0)
                _counter = _weaponCount - 1;
            else
                _counter--;
            
            _weaponText.text = $"{_weaponList[_counter].name}\n#{_counter}";
            
            _demoWeapon.UpdateWeaponSO(_weaponList[_counter]);
        }

        private void OnLaunchClick() {
            _demoWeapon.LaunchForward();
        }
        
        



    }
}
