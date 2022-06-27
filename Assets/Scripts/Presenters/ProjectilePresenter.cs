using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class ProjectilePresenter : MonoBehaviour
    {
        [SerializeField] Collider _collider;

        [Inject] ProjectileModel _projectile;
        [Inject] ProjectileSpawner _projectileSpawner;
        [Inject] IAudioService _audioService;

        public void Init(Vector3 position, Vector3 direction, float speed)
        {
            // Init model
            _projectile.Init(position, direction, speed);

            // Update position based on model
            _projectile.Position.Subscribe(pos => transform.position = pos).AddTo(this);

            // Enable collider once the starting position is set
            _collider.enabled = true;

            // Set direction
            transform.forward = _projectile.Direction;

            // Update model
            Observable.EveryUpdate().Subscribe(_ =>
            {
                // Despawn projectile when it out of level bounds
                if (_projectile.Move(Time.deltaTime))
                {
                    _projectileSpawner.Despawn(this);
                }
            }).AddTo(this);

            // Handle projectile hit
            this.OnTriggerEnterAsObservable().Subscribe(collider =>
            {
                if (collider.TryGetComponent(out ProjectilePresenter projectile))
                {
                    // Play explosion sfx
                    _audioService.PlaySfx(Constants.Audio.Explosion, 0.25f);

                    // Despawn both projectiles
                    _projectileSpawner.Despawn(projectile);
                    _projectileSpawner.Despawn(this);
                }
            }).AddTo(this);
        }

        public class Factory : PlaceholderFactory<Object, ProjectilePresenter>
        {
        }
    }
}
