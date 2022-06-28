using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class EnemyPresenter : MonoBehaviour, IPoolable<int, int, int, IMemoryPool>, IDisposable
    {
        [SerializeField] GameObject[] _models;

        [Inject] EnemyModel _enemy;
        [Inject] EnemySpawner _enemySpawner;
        [Inject] EnemyConfig _enemyConfig;
        [Inject] ProjectileSpawner _projectileSpawner;
        [Inject] GameplayModel _gameplay;
        [Inject] IAudioService _audioService;
        [Inject] IEnemiesManager _enemiesManager;

        IMemoryPool _pool;

        public Vector3 Position => _enemy.Position;

        private void OnTriggerEnter(Collider other)
        {
            // Handle projectile hit
            if (other.TryGetComponent(out ProjectilePresenter projectile))
            {
                // Play explosion sfx
                _audioService.PlaySfx(Constants.Audio.Explosion, 0.25f);

                // Increase score by enemy type
                _gameplay.CurrentScore.Value += (_enemy.Type + 1) * _enemyConfig.BaseScore;

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
