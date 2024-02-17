using UnityEngine;

namespace Managers {
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







    }
}
