using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders.Services
{
    public class CameraShaker : ICameraShaker
    {
        private readonly Transform _camTransform;
        private readonly Vector3 _originalPos;

        public CameraShaker()
        {
            // Cache camera
            _camTransform = Camera.main.transform;
            _originalPos = _camTransform.position;
        }

        public void Shake(float duration, float strength)
        {
            if (_camTransform == null)
                return;

            // Shake camera
            _camTransform
                .DOShakePosition(duration, strength, 75)
                .OnComplete(() => _camTransform.position = _originalPos);
        }
    }
}
