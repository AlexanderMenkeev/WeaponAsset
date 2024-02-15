using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Camera _camera;
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        _camera = Camera.main;

    }


    public void ExitApplication() {
        Application.Quit();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }







}
