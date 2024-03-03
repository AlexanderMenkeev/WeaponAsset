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
    
    public class AudioManager : MonoBehaviour {

        // Assign in editor
        public AudioClip PlayerShoot;
        public AudioClip PlayerHurt;
        public AudioClip PlayerDeath;
        public AudioClip EnemyShoot;
        public AudioClip EnemyHurt;
        public AudioClip ProjectilesCollision;
        
        [SerializeField] private AudioClip _music1;
        [SerializeField] private AudioClip _music2;
        [SerializeField] private AudioClip _music3;
        [SerializeField] private List<AudioClip> _musicClips = new List<AudioClip>();
        
        private AudioSource _audioSource;
        
        
        public static AudioManager Instance;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }

            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.playOnAwake = false;
            
            _musicClips.AddRange(new[] { _music1, _music2, _music3 });
            _audioSource.clip = _musicClips[Random.Range(0, _musicClips.Count)];
            
            _audioSource.Play();
        }

        [SerializeField] private bool _playAudioEffects = true;
        public void PlayAudioEffect(AudioSource src, AudioClip clip) {
            if (!_playAudioEffects || src.isPlaying)
                return;
            
            src.PlayOneShot(clip);
        }
        
        
        // [SerializeField] private bool _playMusic = true;
        // private void PlayMusic() {
        //     if (_playMusic) {
        //         _audioSource.Play();
        //     }
        //     
        // }
        
        

        












    }
}
