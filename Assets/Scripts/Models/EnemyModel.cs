using System;
using UniRx;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class EnemyConfig
    {
        [Range(1, 6)]
        public int Rows = 5;
        [Range(1, 15)]
        public int Columns = 11;
        public float GridWidth = 2.5f;
        public float GridHeight = 2f;
        public float FireRate = 1f;
        public float SpeedHorizontal = 1f;
        public float SpeedVertical = 1f;
        public float ProjectileSpeed = 10.0f;
        public int BaseScore = 10;
    }

    public class EnemyModel : DisposableEntity
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; private set; }

        private EnemyConfig _enemyConfig;
        private IEnemiesManager _enemiesManager;

        public EnemyModel(EnemyConfig enemyConfig, IEnemiesManager enemiesManager)
        {
            // Set references
            _enemyConfig = enemyConfig;
            _enemiesManager = enemiesManager;
        }

        public void Init(int row, int col)
        {
            // Init properties
            Row = row;
            Col = col;

            // Calculate grid position
            Vector3 gridPos = CalculateGridPosition(Col, Row);

            // Calculate center offset position
            Vector3 centerPos = -CalculateGridPosition((_enemyConfig.Columns - 1) / 2, 0);

            // Modify offset for even number of columns
            if (_enemyConfig.Columns % 2 == 0)
            {
                centerPos -= _enemyConfig.GridWidth / 2f * Vector3.right;
            }

            // Calculate final position
            Position = _enemiesManager.Position.Select(moverPos => moverPos + gridPos + centerPos).ToReadOnlyReactiveProperty();
        }

        /// <summary>
        /// Calculates the grid position of the enemy.
        /// </summary>
        /// <param name="col">The column index of the grid.</param>
        /// <param name="row">The row index of the grid.</param>
        /// <returns>The grid position.</returns>
        public Vector3 CalculateGridPosition(int col, int row)
        {
            return (_enemyConfig.GridWidth * col * Vector3.right) +
                (_enemyConfig.GridHeight * row * Vector3.forward);
        }
    }
}
