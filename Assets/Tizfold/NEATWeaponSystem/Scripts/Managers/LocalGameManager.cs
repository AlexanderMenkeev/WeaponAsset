using System;
using System.Collections;
using System.Collections.Generic;
using Tizfold.NEATWeaponSystem.Scripts.EnemyBehaviour;
using Tizfold.NEATWeaponSystem.Scripts.Interfaces;
using Tizfold.NEATWeaponSystem.Scripts.SODefinitions;
using Tizfold.NEATWeaponSystem.Scripts.WeaponSystem.NEAT;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Tizfold.NEATWeaponSystem.Scripts.Managers {
    
    // Enemy spawning, PauseCanvas activation and common functions
    public class LocalGameManager : MonoBehaviour {

        // Assign in editor
        [SerializeField] private GlobalVariablesSO _globalVariables;
        [SerializeField] private List<Sprite> _enemySprites;
        [SerializeField] private WeaponParamsSO[] _weaponSoList;
        public GameObject EnemyPrefab;

        public static LocalGameManager Instance;
        [SerializeField] private Camera _camera;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
            _camera = Camera.main;

            foreach (WeaponParamsSO weaponParams in _weaponSoList) {
                weaponParams.LoadParamsFromJson();
                weaponParams.Lifespan = 4f;
                weaponParams.ProjectilesInOneShot = 10;
                weaponParams.ForwardForce = true;
                weaponParams.Mode = ProjectileMode.Polar;
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
                
                List<Vector3> posList = new List<Vector3>();
                Vector3 posOnLeftBand = new Vector3(Random.Range(p00.x - _xMargin, p00.x), Random.Range(p00.y - _yMargin, p11.y + _yMargin), 0);
                Vector3 posOnRightBand = new Vector3(Random.Range(p11.x, p11.x + _xMargin), Random.Range(p00.y - _yMargin, p11.y + _yMargin), 0);
                Vector3 posOnUpperBand = new Vector3(Random.Range(p00.x - _xMargin, p11.x + _xMargin), Random.Range(p11.y, p11.y + _yMargin), 0);
                Vector3 posOnLowerBand = new Vector3(Random.Range(p00.x - _xMargin, p11.x + _xMargin), Random.Range(p00.y - _yMargin, p00.y), 0);
                posList.AddRange(new []{posOnLeftBand, posOnRightBand, posOnUpperBand, posOnLowerBand});
                
                EnemyController enemy = Instantiate(EnemyPrefab, posList[Random.Range(0, posList.Count)], Quaternion.identity, transform).
                    GetComponent<EnemyController>();
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
        private InputAction _pauseAction;
        private InputManager _inputManager;

        private void Initialize() {
            _inputManager = InputManager.Instance;
            _pauseAction = _inputManager.PauseAction;
        }

        private void Start() {
            Initialize();
            _spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
            _pauseAction.started += Pause;
            _globalVariables.OnPauseResumeEvent += OnPauseResumeGame;
        }

        private void OnDestroy() {
            _pauseAction.started -= Pause;
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
        /// Change value isPaused in GlobalVariables
        /// </summary>
        public void Pause(bool value) {
            _globalVariables.IsPaused = value;
        }

        public void DamageObject(IDamagable objectToDamage, float damage) {
            if (objectToDamage.HealthPoints <= 0f)
                return;
            objectToDamage.TakeDamage(damage);
        }












    }
}
