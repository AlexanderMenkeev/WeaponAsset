using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject _loaderCanvas;
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            
        }
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        
        _loaderCanvas.SetActive(true);
        await Task.Delay(3000);
        while (scene.progress < 0.9f) {
            Debug.Log(scene.progress);
            await Task.Yield();
        }
        
        scene.allowSceneActivation = true;
        await Task.Delay(1000);
        _loaderCanvas.SetActive(false);
    }
}
