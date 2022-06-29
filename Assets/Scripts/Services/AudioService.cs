using System;
using UnityEngine;
using DG.Tweening;

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
        Tween _tween;

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

            // Create audio source if for the first time
            if (_music == null)
            {
                var go = new GameObject("Music");// todo: create only once...
                _music = go.AddComponent<AudioSource>();
                _music.spatialBlend = 0;
                _music.volume = 0;
                _music.loop = true;
            }

            // Fade int music
            _tween?.Kill();
            _tween = _music.DOFade(volume, 2f)
                .SetEase(Ease.InQuad)
                .OnStart(() =>
                {
                    _music.clip = clip;
                    _music.volume = 0f;
                    _music.Play();
                })
                .OnComplete(() =>
                {
                    _music.volume = volume;
                });
        }

        public void StopMusic()
        {
            // Handle error
            if (_music == null)
            {
                return;
            }

            // Fade out music
            _tween?.Kill();
            _tween = _music.DOFade(0f, 2f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _music.volume = 0f;
                    _music.Stop();
                });

            
        }

        public void DuckMusic(float targetVolume, float originalVolume, float duration)
        {
            // Handle error
            if (_music == null)
            {
                return;
            }

            // Duck music
            _tween?.Kill();
            _tween = _music.DOFade(targetVolume, duration)
                .SetEase(Ease.OutQuad)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _music.volume = originalVolume;
                });
        }
    }
}
