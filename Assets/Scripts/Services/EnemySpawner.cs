using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class EnemySpawner
    {
        private EnemyPresenter.Factory _factory;
        private EnemyConfig _enemyConfig;
        private IAssetService _assetService;

        private List<EnemyPresenter> _enemies = new List<EnemyPresenter>();

        public EnemySpawner(EnemyPresenter.Factory factory, EnemyConfig enemyConfig, IAssetService assetService)
        {
            // Set references
            _factory = factory;
            _enemyConfig = enemyConfig;
            _assetService = assetService;
        }

        public IReadOnlyList<EnemyPresenter> Enemies => _enemies.AsReadOnly();

        public void SpawnAll()
        {
            List<GameObject> prefabs = new List<GameObject>(3);

            // Try to get enemy prefabs
            for (int i = 0; i < 3; i++)
            {
                string key = $"Enemy{i + 1}";
                var prefab = _assetService.Get<GameObject>(key);

                // Stop spawning if an enemy is not found
                if (prefab == null)
                {
                    Debug.LogError($"Couldn't find {key} in the Addressables.");
                    return;
                }

                prefabs.Add(prefab);
            }

            // Spawn enemies
            for (int col = 0; col < _enemyConfig.Columns; col++)
            {
                for (int row = 0; row < _enemyConfig.Rows; row++)
                {
                    // Calculate enemy type based on row
                    int type = row <= 1 ? 0 : row <= 3 ? 1 : 2;

                    // Spawn and init enemy
                    EnemyPresenter enemy = _factory.Create(prefabs[type]);
                    enemy.Init(type, row, col);
                    _enemies.Add(enemy);
                }
            }
        }

        public void DespawnAll()
        {
            // Despawn enemies
            foreach (var enemy in _enemies)
            {
                Object.Destroy(enemy.gameObject);
            }

            // Clear list
            _enemies.Clear();
        }

        public void Despawn(EnemyPresenter enemy)
        {
            // Handle error
            if (enemy == null)
            {
                return;
            }

            // Despawn enemy
            Object.Destroy(enemy.gameObject);

            // Remove from the list
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }
        }
    }
}
