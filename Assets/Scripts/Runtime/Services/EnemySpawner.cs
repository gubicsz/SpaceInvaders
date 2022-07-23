using SpaceInvaders.Models;
using SpaceInvaders.Presenters;
using System.Collections.Generic;

namespace SpaceInvaders.Services
{
    public class EnemySpawner
    {
        readonly EnemyPresenter.Factory _factory;
        readonly EnemyConfig _enemyConfig;

        public List<EnemyPresenter> Enemies { get; private set; } = new List<EnemyPresenter>();

        public EnemySpawner(EnemyPresenter.Factory factory, EnemyConfig enemyConfig)
        {
            // Set references
            _factory = factory;
            _enemyConfig = enemyConfig;
        }

        public void SpawnAll()
        {
            // Spawn enemies
            for (int col = 0; col < _enemyConfig.Columns; col++)
            {
                for (int row = 0; row < _enemyConfig.Rows; row++)
                {
                    // Calculate enemy type based on row
                    int type = row <= 1 ? 0 : row <= 3 ? 1 : 2;

                    // Spawn and init enemy
                    EnemyPresenter enemy = _factory.Create(type, row, col);
                    Enemies.Add(enemy);
                }
            }
        }

        public void DespawnAll()
        {
            // Despawn enemies
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Dispose();
            }

            // Clear list
            Enemies.Clear();
        }

        public void Despawn(EnemyPresenter enemy)
        {
            // Handle error
            if (enemy == null || !Enemies.Contains(enemy))
            {
                return;
            }

            // Despawn enemy
            enemy.Dispose();
            Enemies.Remove(enemy);
        }
    }
}
