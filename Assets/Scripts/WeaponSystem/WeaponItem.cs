using UnityEngine;
using UnityEngine.UI;

namespace WeaponSystem {
    public class WeaponItem : AbstractWeapon
    {
        public GameObject PickUpCanvas;
        public GameObject JoysticksCanvas;
        public CanvasDemoWeapon DemoWeapon;
        public GameSceneWeapon SceneWeapon;
        public Slider DistanceSlider;
        public Toggle Flip;
        public Button AcceptButton;
        public Button DismissButton;
    
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) {
                ActivatePickUpCanvas();
                JoysticksCanvas.SetActive(false);
                LocalGameManager.Instance.Pause(true);
            }
        }
    
        private void ActivatePickUpCanvas() {
            DemoWeapon.Factory = EvolutionAlgorithm.Instance.CppnGenomeFactory;
            DemoWeapon.Decoder = EvolutionAlgorithm.Instance.Decoder;
            DemoWeapon.ProjectileGenome = ProjectileGenome;
            DistanceSlider.value = _weaponParams.MaxDistance;
            Flip.isOn = _weaponParams.FlipY;
        
            PickUpCanvas.SetActive(true);
        
            AcceptButton.onClick.AddListener(OnAcceptWeapon);
            DismissButton.onClick.AddListener(OnDismissWeapon);
            DistanceSlider.onValueChanged.AddListener(OnSliderChanged);
            Flip.onValueChanged.AddListener(OnFlipChanged);
        }

        private void DisablePickUpCanvas() {
            PickUpCanvas.SetActive(false);
            EvolutionAlgorithm.Instance.CreateNewGeneration();
        
            AcceptButton.onClick.RemoveListener(OnAcceptWeapon);
            DismissButton.onClick.RemoveListener(OnDismissWeapon);
            DistanceSlider.onValueChanged.RemoveListener(OnSliderChanged);
            Flip.onValueChanged.RemoveListener(OnFlipChanged);
        }
    
        private void OnAcceptWeapon() {
            SceneWeapon.ProjectileGenome = ProjectileGenome;
            DisablePickUpCanvas();
        
            JoysticksCanvas.SetActive(true);
            LocalGameManager.Instance.Pause(false);
        }
    
        private void OnDismissWeapon() {
            DisablePickUpCanvas();
        
            JoysticksCanvas.SetActive(true);
            LocalGameManager.Instance.Pause(false);
        }
    
        private void OnSliderChanged(float distance) {
            _weaponParams.MaxDistance = distance;
            _weaponParams.UpdateParamsEvent.Invoke();
            DemoWeapon.OnChangeDemoWeaponEvent?.Invoke();
        }
    
    
        private void OnFlipChanged(bool value) {
            _weaponParams.FlipY = value;
            _weaponParams.UpdateParamsEvent.Invoke();
            DemoWeapon.OnChangeDemoWeaponEvent?.Invoke();
        }

    

    
    
    }
}
