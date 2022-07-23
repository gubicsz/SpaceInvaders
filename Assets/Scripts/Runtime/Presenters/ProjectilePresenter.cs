using SpaceInvaders.Models;
using SpaceInvaders.Services;
using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class ProjectilePresenter : MonoBehaviour, IPoolable<Vector3, Vector3, float, IMemoryPool>, IDisposable
    {
        [SerializeField] Collider _collider;

        [Inject] readonly ProjectileModel _projectile;
        [Inject] readonly ProjectileSpawner _projectileSpawner;
        [Inject] readonly IAudioService _audioService;

        IMemoryPool _pool;

        private void Update()
        {
            // Move projectile
            if (_projectile.Move(Time.deltaTime))
            {
                // Despawn projectile when it moved out of level bounds
                _projectileSpawner.Despawn(this);
            }
            else
            {
                // Update position based on model
                transform.position = _projectile.Position;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Handle projectile hit
            if (other.TryGetComponent(out ProjectilePresenter projectile))
            {
                _audioService.PlaySfx(Constants.Audio.Explosion, 0.15f);
                _projectileSpawner.Despawn(projectile);
                _projectileSpawner.Despawn(this);
            }
        }

        public void OnSpawned(Vector3 position, Vector3 direction, float speed, IMemoryPool pool)
        {
            // Init model
            _pool = pool;
            _projectile.Init(position, direction, speed);

            // Set orientation
            transform.position = _projectile.Position;
            transform.forward = _projectile.Direction;

            // Enable collider
            _collider.enabled = true;
        }

        public void OnDespawned()
        {
            // Reset model
            _pool = null;
            _projectile.Reset();

            // Disable collider
            _collider.enabled = false;
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Vector3, Vector3, float, ProjectilePresenter>
        {
        }
    }
}
