using System;
using UniRx;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class ProjectileConfig
    {
        public float Speed = 15.0f;
    }

    public class ProjectileModel
    {
        public Vector3ReactiveProperty Position { get; private set; }
        public Vector3 Direction { get; private set; }

        private ProjectileConfig _projectileConfig;
        private LevelConfig _levelConfig;

        public ProjectileModel(ProjectileConfig projectileConfig, LevelConfig levelConfig)
        {
            // Set references
            _projectileConfig = projectileConfig;
            _levelConfig = levelConfig;
        }

        public void Init(Vector3 position, Vector3 direction)
        {
            Position = new Vector3ReactiveProperty(position);
            Direction = direction;
        }

        public bool Move(float dt)
        {
            // Move projectile
            Position.Value += dt * _projectileConfig.Speed * Direction;

            // Inidicate whether the projectile is out of level bounds or not
            return _levelConfig.IsPosOutOfVerticalBounds(Position.Value);
        }
    }
}
