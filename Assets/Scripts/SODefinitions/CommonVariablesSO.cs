using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CommonVariablesSO")]
public class CommonVariablesSO : ScriptableObject
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
        Debug.Log("OnEnable in CommonVariables is called!");
        IsPaused = false;
        _pauseTime = 0f;
        _resumeTime = 0f;
    }
    
    
    
    
}
