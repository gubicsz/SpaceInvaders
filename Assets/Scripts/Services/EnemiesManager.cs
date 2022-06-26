using UniRx;
using UnityEngine;

namespace SpaceInvaders
{
    public class EnemiesManager
    {
        public Vector3ReactiveProperty Position { get; private set; }
        public Vector3 Direction { get; private set; }

        private LevelConfig _levelConfig;
        private EnemyConfig _enemyConfig;
        private float _lastShotTime;

        public EnemiesManager(LevelConfig levelConfig, EnemyConfig enemyConfig)
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
            _lastShotTime = 0;
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

        public bool Shoot(float time)
        {
            // Prevent shooting within the fire rate period
            if (time < (_lastShotTime + _enemyConfig.FireRate))
            {
                return false;
            }

            // Register last shot time
            _lastShotTime = time;
            return true;
        }
    }
}
