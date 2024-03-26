using SpaceInvaders.Models;
using UnityEngine;

namespace SpaceInvaders.Services
{
    public class EnemiesManager : IEnemiesManager
    {
        private readonly EnemyConfig _enemyConfig;

        private readonly LevelConfig _levelConfig;

        private float _lastShotTime;

        public EnemiesManager(LevelConfig levelConfig, EnemyConfig enemyConfig)
        {
            // Set references
            _levelConfig = levelConfig;
            _enemyConfig = enemyConfig;

            // Set initial values
            Position = Vector3.zero;
            Direction = Vector3.right;
        }

        /// <summary>
        ///     The global position of the formation.
        /// </summary>
        public Vector3 Position { get; private set; }

        public Vector3 Direction { get; private set; }

        public void Reset()
        {
            // Reset properties
            Position = Vector3.zero;
            Direction = Vector3.right;
            _lastShotTime = 0;
        }

        public void Move(Vector3 leftPos, Vector3 rightPos, int enemyCount, float dt)
        {
            // Reverse direction if the level bounds are reached
            if (
                (Direction.x > 0 && _levelConfig.IsPosOutOfHorizontalBounds(rightPos))
                || (Direction.x < 0 && _levelConfig.IsPosOutOfHorizontalBounds(leftPos))
            )
            {
                Direction *= -1f;
                Position += _enemyConfig.SpeedVertical * Vector3.back;
            }

            // Calculate horizontal speed based on the number of enemies.
            // The fewer the enemies the faster they go.
            var enemiesPercent = 1 - enemyCount / (float)(_enemyConfig.Columns * _enemyConfig.Rows);
            var speed =
                (1 + enemiesPercent * _enemyConfig.SpeedMultiplier) * _enemyConfig.SpeedHorizontal;

            // Update position
            Position += dt * speed * Direction;
        }

        public bool Shoot(float time)
        {
            // Prevent shooting within the fire rate period
            if (time < _lastShotTime + _enemyConfig.FireRate)
                return false;

            // Register last shot time
            _lastShotTime = time;
            return true;
        }
    }
}
