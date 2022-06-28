using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class AudioConfig
    {
        [Range(0f, 1f)]
        public float MusicVolume = 0.5f;
    }

    public class AudioService : IAudioService
    {
        IAssetService _assetService;

        Transform _camTransform;
        AudioSource _music;

        public AudioService(IAssetService assetService)
        {
            _assetService = assetService;
        }

        public void PlaySfx(string key, float volume)
        {
            // Get clip from asset service based on key
            AudioClip clip = _assetService.Get<AudioClip>(key);

            // Handle error
            if (clip == null)
            {
                Debug.LogWarning($"Couldn't find '{key}' AudioClip.");
                return;
            }

            // Cache camera transform
            if (_camTransform == null)
            {
                _camTransform = Camera.main.transform;
            }

            // This is just a quick and dirty solution. It is very bad for performance
            // because this call creates and destroys an AudioSource each time.
            // A more professional way would be to use Audio Mixer, buses and pooling.
            AudioSource.PlayClipAtPoint(clip, _camTransform.position, volume);
        }

        public void PlayMusic(string key, float volume)
        {
            // Get clip from asset service based on key
            AudioClip clip = _assetService.Get<AudioClip>(key);

            // Handle error
            if (clip == null)
            {
                Debug.LogWarning($"Couldn't find '{key}' AudioClip.");
                return;
            }

            // Create and play music
            var go = new GameObject("Music");
            _music = go.AddComponent<AudioSource>();
            _music.clip = clip;
            _music.loop = true;
            _music.volume = volume;
            _music.spatialBlend = 0;
            _music.Play();
        }

        public void StopMusic()
        {
            // Handle error
            if (_music == null)
            {
                return;
            }

            // Stop and destroy music
            _music.Stop();
            UnityEngine.Object.Destroy(_music.gameObject);
            _music = null;
        }
    }
}
