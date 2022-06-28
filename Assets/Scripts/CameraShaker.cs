using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceInvaders
{
    public class CameraShaker
    {
        Transform _camTransform;
        Vector3 _originalPos;

        public CameraShaker()
        {
            // Cache camera
            _camTransform = Camera.main.transform;
            _originalPos = _camTransform.position;
        }

        public async void Shake(float duration, float magnitude)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                // Generate random offset based on magnitude
                float x = Random.Range(-1f, 1f) * magnitude;
                float z = Random.Range(-1f, 1f) * magnitude;

                // Shake camera
                _camTransform.position = new Vector3(x, _camTransform.position.y, z);
                elapsed += Time.deltaTime;

                // Wait for end of frame
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            // Restore original position
            _camTransform.position = _originalPos;
        }
    }
}
