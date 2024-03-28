using System.Collections;
using System.Collections.Generic;
using NeatBullets.Core.Scripts.Interfaces;
using NeatBullets.Core.Scripts.SODefinitions;
using NeatBullets.Core.Scripts.WeaponSystem;
using NeatBullets.GameExample.Scripts.EnemyBehaviour;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace NeatBullets.GameExample.Scripts.Managers {
    
    // Enemy spawning, Pause and Death canvases activation, invoking of TakeDamage
    public class GameManager : MonoBehaviour {

        // Assign in editor
        [SerializeField] private GlobalVariablesSO _globalVariables;
        [SerializeField] private List<Sprite> _enemySprites;
        [SerializeField] private WeaponParamsSO[] _weaponSoList;
        [SerializeField] private GameObject _enemyPrefab;

        public static GameManager Instance;
        [HideInInspector] [SerializeField] private Camera _camera;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
            _camera = Camera.main;

            
            foreach (WeaponParamsSO weaponParams in _weaponSoList) {
                // Reset SO values to json's
                weaponParams.LoadParamsFromJson();
                
                // Override values
                weaponParams.Lifespan = 4f;
                weaponParams.ProjectilesInOneShot = 8;
                weaponParams.ForwardForce = true;
                weaponParams.Mode = ReflectionMode.Polar;
                weaponParams.InitialSpeed = Random.Range(3f, 5f);
                weaponParams.MaxPolarAngleDeg = Random.Range(5f, 45f);
                weaponParams.Angle = Random.Range(2f, weaponParams.MaxPolarAngleDeg);
                weaponParams.NNControlDistance = 8f;
            }
        }
        
        
        private Coroutine _spawnEnemiesCoroutine;

        [Header("Enemy spawn")] 
        [SerializeField] [Range(1f, 5f)] private float _xMargin = 1f;
        [SerializeField] [Range(1f, 5f)] private float _yMargin = 1f;
        [SerializeField] [Range(2f, 10f)] private float _spawnRate = 4f;
        private IEnumerator SpawnEnemies() {
            while (true) {
                yield return new WaitForSeconds(_spawnRate * 0.5f);
                Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
                Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
                
                // Rectangle with camera view hole inside, where enemies cannot spawn
                List<Vector3> posList = new List<Vector3>();
                Vector3 posOnLeftBand = new Vector3(Random.Range(p00.x - _xMargin, p00.x), Random.Range(p00.y - _yMargin, p11.y + _yMargin), 0);
                Vector3 posOnRightBand = new Vector3(Random.Range(p11.x, p11.x + _xMargin), Random.Range(p00.y - _yMargin, p11.y + _yMargin), 0);
                Vector3 posOnUpperBand = new Vector3(Random.Range(p00.x - _xMargin, p11.x + _xMargin), Random.Range(p11.y, p11.y + _yMargin), 0);
                Vector3 posOnLowerBand = new Vector3(Random.Range(p00.x - _xMargin, p11.x + _xMargin), Random.Range(p00.y - _yMargin, p00.y), 0);
                posList.AddRange(new []{posOnLeftBand, posOnRightBand, posOnUpperBand, posOnLowerBand});
                
                EnemyController enemy = Instantiate(_enemyPrefab, posList[Random.Range(0, posList.Count)], Quaternion.identity, transform).
                    GetComponent<EnemyController>();
                
                // Transfer SOs and randomize stats
                enemy.Sprite = _enemySprites[Random.Range(0, _enemySprites.Count)];
                enemy.Weapon.UpdateWeaponSO(_weaponSoList[Random.Range(0, _weaponSoList.Length)]);
                enemy.HealthPoints = Random.Range(5f, 20f);
                enemy.MovementSpeed = Random.Range(2f, 3f);
                enemy.AttackDistance = Random.Range(5f, 8f);
                
                yield return new WaitForSeconds(_spawnRate * 0.5f);
            }
        }

        // Visualize enemy spawn zone
        private void OnDrawGizmos() {
            if (_camera == null)
                return;
            
            Vector2 p11 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            Vector2 p00 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            
            Vector3[] points = new Vector3[4] {
                // lowerLeftPoint
                new Vector3(p00.x - _xMargin, p00.y - _yMargin, 0), 
                // lowerRightPoint
                new Vector3(p11.x + _xMargin, p00.y - _yMargin, 0),
                // upperRightPoint
                new Vector3(p11.x + _xMargin, p11.y + _yMargin, 0),
                // upperLeftPoint
                new Vector3(p00.x - _xMargin, p11.y + _yMargin, 0)
            };
            
            Gizmos.DrawLineStrip(points, true);
        }

        [SerializeField] private GameObject _pauseCanvas;
        [SerializeField] private GameObject _deathCanvas;
        private InputAction _pauseAction;
        private InputManager _inputManager;

        private void Initialize() {
            _inputManager = InputManager.Instance;
            _pauseAction = _inputManager.PauseAction;
        }

        private void Start() {
            Initialize();
            _spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
            _pauseAction.started += ActivatePauseCanvas;
            _globalVariables.OnPauseResumeEvent += OnPauseResumeGame;
        }

        private void OnDestroy() {
            _pauseAction.started -= ActivatePauseCanvas;
            _globalVariables.OnPauseResumeEvent -= OnPauseResumeGame;
        }
        
        private void OnPauseResumeGame() {
            if (_globalVariables.IsPaused)
                StopCoroutine(_spawnEnemiesCoroutine);
            else
                _spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        }

        
        /// <summary>
        /// Activate and deactivate pause canvas on user input (Esc on PC or Back on mobile)
        /// </summary>
        private void ActivatePauseCanvas(InputAction.CallbackContext context) {
            if (!_pauseCanvas.activeSelf) {
                _pauseCanvas.SetActive(true);
                Pause(true);
            }
            else {
                _pauseCanvas.SetActive(false);
                Pause(false);
            }
        }
        
        // For resume button
        public void ActivatePauseCanvas() {
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
        /// Change value isPaused in GlobalVariables
        /// </summary>
        public void Pause(bool value) {
            _globalVariables.IsPaused = value;
        }

        /// <summary>
        /// Invoke TakeDamage method of IDamagable object, if its HPs are nonnegative
        /// </summary>
        public void DamageObject(IDamagable objectToDamage, float damage) {
            if (objectToDamage.HealthPoints <= 0f)
                return;
            objectToDamage.TakeDamage(damage);
        }
        
        
        public void ActivateDeathCanvas() {
            if (!_deathCanvas.activeSelf) {
                _deathCanvas.SetActive(true);
                Pause(true);
            }
            else {
                _deathCanvas.SetActive(false);
                Pause(false);
            }
        }

        











    }
}
