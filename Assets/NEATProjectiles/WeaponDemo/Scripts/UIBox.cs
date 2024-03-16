using System;
using NeatProjectiles.Core.Scripts.SODefinitions;
using NeatProjectiles.Core.Scripts.WeaponSystem;
using SharpNeat.Phenomes;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace NeatProjectiles.WeaponDemo.Scripts {
    public class UIBox : MonoBehaviour {
        
        // assigned from the editor
        [SerializeField] private Button _nextBtn;
        [SerializeField] private Button _prevBtn;
        [SerializeField] private Button _forceViewBtn;
        [SerializeField] private Button _launchBtn;
        [SerializeField] private TextMeshProUGUI _weaponText;
        [SerializeField] private DemoWeapon _demoWeapon;
        [SerializeField] private WeaponParamsSO[] _weaponList;
        [SerializeField] private ArrowSpawner _arrowSpawner;
        
        
        private int _counter;
        private int _weaponCount;
        private void Awake() {
            _counter = 0;
            _weaponCount = _weaponList.Length;

            foreach (WeaponParamsSO so in _weaponList) {
                so.LoadParamsFromJson();
                so.Lifespan = 10f;
            }
            
            _nextBtn.onClick.AddListener(OnNextBtnClick);
            _prevBtn.onClick.AddListener(OnPrevBtnClick);
            _forceViewBtn.onClick.AddListener(OnForceViewClick);
            _launchBtn.onClick.AddListener(OnLaunchClick);
        }

        private void OnDestroy() {
            _nextBtn.onClick.RemoveListener(OnNextBtnClick);
            _prevBtn.onClick.RemoveListener(OnPrevBtnClick);
            _forceViewBtn.onClick.RemoveListener(OnForceViewClick);
            _launchBtn.onClick.RemoveListener(OnLaunchClick);
        }
        
        private void Start() {
            _arrowSpawner.CreateForceMap();
            UpdateTextAndParams();
        }

        private void OnNextBtnClick() {
            if (_counter == _weaponCount - 1)
                _counter = 0;
            else
                _counter++;

            UpdateTextAndParams();
        }
        
        private void OnPrevBtnClick() {
            if (_counter == 0)
                _counter = _weaponCount - 1;
            else
                _counter--;
            
            UpdateTextAndParams();
        }

        private bool _arrowsVisible = false;
        private void OnForceViewClick() {
            
            //_demoWeapon.StopAllCoroutines();
            if (!_arrowsVisible) {
                _weaponList[_counter].DestroyProjectilesEvent?.Invoke();
                
                ArrowsSpritesEnabled(true);
                _arrowsVisible = true;
            }
            else {
                //_demoWeapon.FireCoroutine = _demoWeapon.StartCoroutine(_demoWeapon.Fire());
                ArrowsSpritesEnabled(false);
                _arrowsVisible = false;
            }
            
        }

        private void ArrowsSpritesEnabled(bool value) {
            foreach (GameObject go in _arrowSpawner.ArrowList) {
                ForceArrow arrow = go.GetComponent<ForceArrow>();
                foreach (SpriteRenderer sr in arrow.SpriteRenderers) 
                    sr.enabled = value;
            }
            
        }

        private void OnLaunchClick() {
            _demoWeapon.LaunchForward();
        }

        private void UpdateTextAndParams() {
            _weaponText.text = $"{_weaponList[_counter].name}\n#{_counter}";
            _demoWeapon.UpdateWeaponSO(_weaponList[_counter], !_arrowsVisible);
            UpdateArrows(_weaponList[_counter]);
        }
        
        private void UpdateArrows(WeaponParamsSO weaponSO) {
            foreach (GameObject go in _arrowSpawner.ArrowList) { 
                ForceArrow arrow = go.GetComponent<ForceArrow>();

                arrow.WeaponParamsLocal = new WeaponParams(weaponSO);
                arrow.OriginTransform = _demoWeapon.ProjectileSpawnPoint;

                arrow.SignX = weaponSO.SignX;
                arrow.SignY = weaponSO.SignY;

                arrow.Box = _demoWeapon.GenomeStats.Box;
                arrow.InputArr = _demoWeapon.GenomeStats.Box.InputSignalArray;
                arrow.OutputArr = _demoWeapon.GenomeStats.Box.OutputSignalArray;

                arrow.CalcProjectileStats();
                switch (weaponSO.Mode) {
                    case ReflectionMode.CircleReflection:
                        arrow.ActivateBlackBoxCircle();
                        break;
                    case ReflectionMode.RectangleReflection:
                        arrow.ActivateBlackBoxRect();
                        break;
                    case ReflectionMode.Polar:
                        arrow.ActivateBlackBoxPolar();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                arrow.ReadDataFromBlackBox();
            }
            
        }
        



    }
}
