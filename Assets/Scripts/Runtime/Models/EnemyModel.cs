using System;
using SpaceInvaders.Helpers;
using UnityEngine;

namespace SpaceInvaders.Models
{
    [Serializable]
    public class EnemyConfig
    {
        [Tooltip("Number of rows in the grid.")]
        [Range(1, 6)]
        public int Rows = 5;

        [Tooltip("Number of columns in the grid.")]
        [Range(1, 15)]
        public int Columns = 11;

        [Tooltip("Width of a cell in the grid.")]
        public float GridWidth = 2.5f;

        [Tooltip("Height of a cell in the grid.")]
        public float GridHeight = 2f;

        [Tooltip("Time between enemy projectiles in seconds.")]
        public float FireRate = 1f;

        [Tooltip("Horizontal speed in m/s.")]
        public float SpeedHorizontal = 1f;

        [Tooltip("Vertical speed in m/s.")]
        public float SpeedVertical = 1f;

        [Tooltip(
            "Multiplies horizontal speed based on the numer of enemies. The fewer the enemies the faster they go."
        )]
        [Range(1f, 5f)]
        public float SpeedMultiplier = 2f;

        [Tooltip("Speed of an enemy projectile in m/s.")]
        public float ProjectileSpeed = 10.0f;

        [Tooltip("Base score of an enemy. THis is multiplied by the enemy type.")]
        public int BaseScore = 10;
    }

    public class EnemyModel : DisposableEntity
    {
        private readonly EnemyConfig _enemyConfig;

        public EnemyModel(EnemyConfig enemyConfig)
        {
            // Set references
            _enemyConfig = enemyConfig;
        }

        public int Type { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Vector3 Position { get; private set; }

        public void Init(int type, int row, int col)
        {
            // Init properties
            Type = type;
            Row = row;
            Col = col;

            // Calculate grid position
            var gridPos = CalculateGridPosition(Col, Row);

            // Calculate center offset position
            var centerPos = -CalculateGridPosition((_enemyConfig.Columns - 1) / 2, 0);

            // Modify offset for even number of columns
            if (_enemyConfig.Columns % 2 == 0)
                centerPos -= _enemyConfig.GridWidth / 2f * Vector3.right;

            // Calculate final position
            Position = gridPos + centerPos;
        }

        public void Reset()
        {
            // Reset properties
            Row = 0;
            Col = 0;
            Type = 0;
            Position = Vector3.zero;
        }

        /// <summary>
        ///     Calculates the grid position of the enemy.
        /// </summary>
        /// <param name="col">The column index of the grid.</param>
        /// <param name="row">The row index of the grid.</param>
        /// <returns>The grid position.</returns>
        public Vector3 CalculateGridPosition(int col, int row)
        {
            return _enemyConfig.GridWidth * col * Vector3.right
                + _enemyConfig.GridHeight * row * Vector3.forward;
        }
    }
}
