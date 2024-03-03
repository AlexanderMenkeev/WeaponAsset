using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace Tizfold.NEATWeaponSystem.Scripts.UI {
    public class FloatingStickAreaController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public GameObject Joystick;
        private OnScreenStick _screenStick;
        private RectTransform _joystickRect;

        // список images, которые мы скрываем
        private Image[] _images;
    
        private void Awake() {
            _screenStick = Joystick.GetComponentInChildren<OnScreenStick>();
            _joystickRect = Joystick.GetComponent<RectTransform>();
            _images = Joystick.GetComponentsInChildren<Image>();
        }

        private void Start() {
            foreach (Image image in _images) {
                image.enabled = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            //Debug.Log("The mouse click started " + gameObject.name);
            foreach (Image image in _images) {
                image.enabled = true;
            }
        
            _joystickRect.position = eventData.position;
            ExecuteEvents.pointerDownHandler(_screenStick, eventData);
        }
    
    
        public void OnDrag(PointerEventData eventData)
        {
            ExecuteEvents.dragHandler(_screenStick, eventData);
        }
    
    
        public void OnPointerUp(PointerEventData eventData)
        {
            ExecuteEvents.pointerUpHandler(_screenStick, eventData);
        
            foreach (Image image in _images) {
                image.enabled = false;
            }
        
            //Debug.Log("The mouse click was released " + gameObject.name);
        }
    }
}