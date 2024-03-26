using System.Collections.Generic;
using SpaceInvaders.Models;
using SpaceInvaders.Presenters;

namespace SpaceInvaders.Services
{
    public class EnemySpawner : IEnemySpawner
    {
        private readonly EnemyConfig _enemyConfig;
        private readonly EnemyPresenter.Factory _factory;

        public EnemySpawner(EnemyPresenter.Factory factory, EnemyConfig enemyConfig)
        {
            // Set references
            _factory = factory;
            _enemyConfig = enemyConfig;
        }

        public List<EnemyPresenter> Enemies { get; } = new();

        public void SpawnAll()
        {
            // Spawn enemies
            for (var col = 0; col < _enemyConfig.Columns; col++)
            for (var row = 0; row < _enemyConfig.Rows; row++)
            {
                // Calculate enemy type based on row
                var type =
                    row <= 1
                        ? 0
                        : row <= 3
                            ? 1
                            : 2;

                // Spawn and init enemy
                var enemy = _factory.Create(type, row, col);
                Enemies.Add(enemy);
            }
        }

        public void DespawnAll()
        {
            // Despawn enemies
            for (var i = 0; i < Enemies.Count; i++)
                Enemies[i].Dispose();

            // Clear list
            Enemies.Clear();
        }

        public void Despawn(EnemyPresenter enemy)
        {
            // Handle error
            if (enemy == null || !Enemies.Contains(enemy))
                return;

            // Despawn enemy
            enemy.Dispose();
            Enemies.Remove(enemy);
        }
    }
}
