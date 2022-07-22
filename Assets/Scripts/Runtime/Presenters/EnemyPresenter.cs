using SpaceInvaders.Models;
using SpaceInvaders.Services;
using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class EnemyPresenter : MonoBehaviour, IPoolable<int, int, int, IMemoryPool>, IDisposable
    {
        [SerializeField] GameObject[] _models;

        [Inject] readonly EnemyModel _enemy;
        [Inject] readonly GameplayModel _gameplay;
        [Inject] readonly EnemyConfig _enemyConfig;
        [Inject] readonly EnemySpawner _enemySpawner;
        [Inject] readonly ProjectileSpawner _projectileSpawner;
        [Inject] readonly ExplosionSpawner _explosionSpawner;
        [Inject] readonly IAudioService _audioService;

        IMemoryPool _pool;

        /// <summary>
        /// The local position of the enemy in the formation.
        /// </summary>
        public Vector3 Position => _enemy.Position;

        private void OnTriggerEnter(Collider other)
        {
            // Handle projectile hit
            if (other.TryGetComponent(out ProjectilePresenter projectile))
            {
                // Increase score by enemy type
                _gameplay.CurrentScore.Value += (_enemy.Type + 1) * _enemyConfig.BaseScore;

                // Play explosion sfx
                _audioService.PlaySfx(Constants.Audio.Explosion, 0.15f);

                // Spawn explosion
                _explosionSpawner.Spawn(transform.position);

                // Despawn projectile
                _projectileSpawner.Despawn(projectile);

                // Despawn self
                _enemySpawner.Despawn(this);
            }
        }

        public void OnSpawned(int type, int row, int col, IMemoryPool pool)
        {
            // Init model
            _pool = pool;
            _enemy.Init(type, row, col);

            // Show model based on type
            _models[_enemy.Type].SetActive(true);
        }

        public void OnDespawned()
        {
            // Hide models
            _models[_enemy.Type].SetActive(false);

            // Reset model
            _pool = null;
            _enemy.Reset();
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<int, int, int, EnemyPresenter>
        {
        }
    }
}
