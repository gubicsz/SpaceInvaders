using System;
using SpaceInvaders.Models;
using SpaceInvaders.Services;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class EnemyPresenter : MonoBehaviour, IPoolable<int, int, int, IMemoryPool>, IDisposable
    {
        [SerializeField] private GameObject[] _models;
        [Inject] private readonly IAudioService _audioService;

        [Inject] private readonly EnemyModel _enemy;
        [Inject] private readonly EnemyConfig _enemyConfig;
        [Inject] private readonly IEnemySpawner _enemySpawner;
        [Inject] private readonly IExplosionSpawner _explosionSpawner;
        [Inject] private readonly GameplayModel _gameplay;
        [Inject] private readonly IProjectileSpawner _projectileSpawner;

        private IMemoryPool _pool;

        /// <summary>
        ///     The local position of the enemy in the formation.
        /// </summary>
        public Vector3 Position => _enemy.Position;

        private void OnTriggerEnter(Collider other)
        {
            // Handle projectile hit
            if (other.TryGetComponent(out ProjectilePresenter projectile))
            {
                _gameplay.CurrentScore.Value += (_enemy.Type + 1) * _enemyConfig.BaseScore;
                _audioService.PlaySfx(Constants.Audio.Explosion, 0.15f);
                _explosionSpawner.Spawn(transform.position);
                _projectileSpawner.Despawn(projectile);
                _enemySpawner.Despawn(this);
            }
        }

        public void Dispose()
        {
            _pool.Despawn(this);
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

        public class Factory : PlaceholderFactory<int, int, int, EnemyPresenter>
        {
        }
    }
}