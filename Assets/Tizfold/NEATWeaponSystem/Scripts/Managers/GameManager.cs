using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tizfold.NEATWeaponSystem.Scripts.Managers {
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    
        private void Start() {
            Application.targetFrameRate = 60;
        }
        
        public void LoadScene(string sceneName) {
            SceneManager.LoadSceneAsync(sceneName);
        }
        


    }
}
