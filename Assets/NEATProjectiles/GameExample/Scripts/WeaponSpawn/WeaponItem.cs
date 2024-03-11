using NEATProjectiles.Core.Scripts.SODefinitions;
using NEATProjectiles.Core.Scripts.WeaponSystem.NEAT;
using NEATProjectiles.Demos.Scripts.Player;
using NEATProjectiles.Demos.Scripts.UIScripts;
using UnityEngine;
using UnityEngine.UI;

namespace NEATProjectiles.Demos.Scripts.WeaponSpawn {
    public class WeaponItem : MonoBehaviour
    {
        // assigned from WeaponSpawner
        public WeaponParamsSO WeaponSO;
        public GenomeStats GenomeStats;
        public GameObject PickUpCanvas;
        public GameObject JoysticksCanvas;
        public CanvasWeapon CanvasWeapon;
        public GameWeapon GameWeapon;
        public Slider DistanceSlider;
        public Toggle Flip;
        public Button AcceptButton;
        public Button DismissButton;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player"))
                return;
            
            ActivatePickUpCanvas();
            JoysticksCanvas.SetActive(false);
            //LocalGameManager.Instance.Pause(true);
        }
    
        private void ActivatePickUpCanvas() {
            
            DistanceSlider.value = WeaponSO.NNControlDistance;
            Flip.isOn = WeaponSO.SignY > 0;
        
            PickUpCanvas.SetActive(true);
            
            
            CanvasWeapon.GenomeStats = new GenomeStats(this.GenomeStats.Genome, EvolutionAlgorithm.Instance.Decoder, EvolutionAlgorithm.Instance.CppnGenomeFactory);
            
            CanvasWeapon.FireCoroutine = StartCoroutine(CanvasWeapon.Fire());
        
            
            AcceptButton.onClick.AddListener(OnAcceptWeapon);
            DismissButton.onClick.AddListener(OnDismissWeapon);
            DistanceSlider.onValueChanged.AddListener(OnSliderChanged);
            Flip.onValueChanged.AddListener(OnFlipChanged);
        }

        private void DisablePickUpCanvas() {
            PickUpCanvas.SetActive(false);
            StopCoroutine(CanvasWeapon.FireCoroutine);
            EvolutionAlgorithm.Instance.CreateRandomPopulation();
        
            AcceptButton.onClick.RemoveListener(OnAcceptWeapon);
            DismissButton.onClick.RemoveListener(OnDismissWeapon);
            DistanceSlider.onValueChanged.RemoveListener(OnSliderChanged);
            Flip.onValueChanged.RemoveListener(OnFlipChanged);
        }
    
        private void OnAcceptWeapon() {
            GameWeapon.GenomeStats = new GenomeStats(this.GenomeStats.Genome, EvolutionAlgorithm.Instance.Decoder, EvolutionAlgorithm.Instance.CppnGenomeFactory);
            DisablePickUpCanvas();
        
            JoysticksCanvas.SetActive(true);
            //LocalGameManager.Instance.Pause(false);
        }
    
        private void OnDismissWeapon() {
            DisablePickUpCanvas();
        
            JoysticksCanvas.SetActive(true);
            //LocalGameManager.Instance.Pause(false);
        }
    
        private void OnSliderChanged(float distance) {
            WeaponSO.NNControlDistance = distance;
            WeaponSO.UpdateParamsEvent?.Invoke();
        }
        
        private void OnFlipChanged(bool value) {
            WeaponSO.SignY = value ? 1f : -1f;
            WeaponSO.UpdateParamsEvent?.Invoke();
        }

    

    
    
    }
}
