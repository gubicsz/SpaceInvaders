using UnityEngine;

namespace SpaceInvaders
{
    public class AudioService : IAudioService
    {
        IAssetService _assetService;

        Transform _camTransform;

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
            AudioSource.PlayClipAtPoint(clip, _camTransform.position, volume);
        }
    }
}
