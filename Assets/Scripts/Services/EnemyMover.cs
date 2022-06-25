using UniRx;
using UnityEngine;

namespace SpaceInvaders
{
    public class EnemyMover
    {
        public Vector3ReactiveProperty Position { get; private set; }
        public Vector3 Direction { get; private set; }

        private LevelConfig _levelConfig;
        private EnemyConfig _enemyConfig;

        public EnemyMover(LevelConfig levelConfig, EnemyConfig enemyConfig)
        {
            // Set references
            _levelConfig = levelConfig;
            _enemyConfig = enemyConfig;

            // Set initial values
            Position = new Vector3ReactiveProperty(Vector3.zero);
            Direction = Vector3.right;
        }

        public void Reset()
        {
            // Reset properties
            Position.Value = Vector3.zero;
            Direction = Vector3.right;
        }

        public void Move(Vector3 leftPos, Vector3 rightPos, float dt)
        {
            // Reverse direction if the level bounds are reached
            if ((Direction.x > 0 && _levelConfig.IsPosOutOfHorizontalBounds(rightPos)) ||
                (Direction.x < 0 && _levelConfig.IsPosOutOfHorizontalBounds(leftPos)))
            {
                Direction *= -1f;
                Position.Value += _enemyConfig.SpeedVertical * Vector3.back;
            }

            // Update position
            Position.Value += dt * _enemyConfig.SpeedHorizontal * Direction;
        }
    }
}
