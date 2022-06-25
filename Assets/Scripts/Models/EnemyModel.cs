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
        public float Width = 2f;
        public float Height = 2f;
        public float FireRate = 0.5f;
        public float SpeedHorizontal = 1f;
        public float SpeedVertical = 1f;
    }

    public class EnemyModel : DisposableEntity
    {
        public int Type { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; private set; }

        private EnemyConfig _enemyConfig;
        private EnemyMover _enemyMover;

        public EnemyModel(EnemyConfig enemyConfig, EnemyMover enemyMover)
        {
            // Set references
            _enemyConfig = enemyConfig;
            _enemyMover = enemyMover;
        }

        public void Init(int type, int row, int col)
        {
            Type = type;
            Row = row;
            Col = col;

            // Calculate grid position
            Vector3 gridPos = CalculateGridPosition(Col, Row);

            // Calculate center offset position
            Vector3 centerPos = -CalculateGridPosition((_enemyConfig.Columns - 1) / 2, 0);

            if (_enemyConfig.Columns % 2 == 0)
            {
                centerPos -= _enemyConfig.Width / 2f * Vector3.right;
            }

            // Calculate final position
            Position = _enemyMover.Position.Select(moverPos => moverPos + gridPos + centerPos).ToReadOnlyReactiveProperty();
        }

        private Vector3 CalculateGridPosition(int col, int row)
        {
            return (_enemyConfig.Width * col * Vector3.right) +
                (_enemyConfig.Height * row * Vector3.forward);
        }
    }
}
