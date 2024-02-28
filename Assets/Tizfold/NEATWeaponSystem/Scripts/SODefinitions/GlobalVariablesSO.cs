using System;
using UnityEngine;

namespace Tizfold.NEATWeaponSystem.Scripts.SODefinitions {
    [CreateAssetMenu(menuName = "ScriptableObjects/CommonVariablesSO")]
    public class GlobalVariablesSO : ScriptableObject
    {
        public event Action OnPauseResumeEvent;
        [SerializeField] private bool _isPaused; 
        public bool IsPaused {
            get => _isPaused; 
            set {
                _isPaused = value;
            
                if (value)
                    _pauseTime = Time.time;
                else 
                    _resumeTime = Time.time;
            
                OnPauseResumeEvent?.Invoke();
            }  
        }
    
        [SerializeField] private float _pauseTime;
        [SerializeField] private float _resumeTime;
        public float PauseTime => _pauseTime;
        public float ResumeTime => _resumeTime;
        public float PauseDuration {
            get {
                float duration = _resumeTime - _pauseTime;
                if (duration < 0)
                    throw new Exception("Pause duration is negative!");
            
                return duration;
            }
        }
    
        public void OnEnable() {
            IsPaused = false;
            _pauseTime = 0f;
            _resumeTime = 0f;
        }
    
    
    
    
    }
}
