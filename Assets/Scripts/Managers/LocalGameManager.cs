using System.Collections;
using Interfaces;
using SODefinitions;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LocalGameManager : MonoBehaviour {
    public static LocalGameManager Instance;
    private Camera _camera;

    [SerializeField] private CommonVariablesSO _commonVariables;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        _camera = Camera.main;
    }

    public Coroutine SpawnEnemiesCoroutine;

    public GameObject EnemyPrefab;

    private IEnumerator SpawnEnemies() {
        while (true) {
            yield return new WaitForSeconds(4f);
            Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));

            Vector3 pos = new Vector3(Random.Range(p00.x, p11.x), Random.Range(p00.y, p11.y), 0);
            Instantiate(EnemyPrefab, pos, Quaternion.identity, transform);
        }
    }

    [SerializeField] private GameObject _pauseCanvas;
    private InputAction _pauseAction;
    private InputManager _inputManager;

    private void Initialize() {
        _inputManager = InputManager.Instance;
        _pauseAction = _inputManager.PauseAction;
    }

    private void Start() {
        Initialize();
        SpawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        _pauseAction.started += Pause;
        _commonVariables.OnPauseResumeEvent += OnPauseResumeGame;
    }

    private void OnDestroy() {
        _pauseAction.started -= Pause;
        _commonVariables.OnPauseResumeEvent -= OnPauseResumeGame;
    }

    public void OnPauseResumeGame() {
        if (_commonVariables.IsPaused)
            StopCoroutine(SpawnEnemiesCoroutine);
        else
            SpawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());



    }


    
    /// <summary>
    /// Activate pause canvas on user input
    /// </summary>
    private void Pause(InputAction.CallbackContext context) {
        if (!_pauseCanvas.activeSelf) {
            _pauseCanvas.SetActive(true);
            Pause(true);
        }
        else {
            _pauseCanvas.SetActive(false);
            Pause(false);
        }
    }

    /// <summary>
    /// Change value isPaused in CommonVariables
    /// </summary>
    public void Pause(bool value) {
        _commonVariables.IsPaused = value;
    }

    public void DamageObject(IDamagable objectToDamage, float damage) {
        if (objectToDamage.HealthPoints <= 0f)
            return;
        objectToDamage.TakeDamage(damage);
    }












}
