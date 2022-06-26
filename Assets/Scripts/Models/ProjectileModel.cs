using System;
using UniRx;
using UnityEngine;

namespace SpaceInvaders
{
    public class ProjectileModel
    {
        public Vector3ReactiveProperty Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Speed { get; private set; }

        private LevelConfig _levelConfig;

        public ProjectileModel(LevelConfig levelConfig)
        {
            // Set references
            _levelConfig = levelConfig;
        }

        public void Init(Vector3 position, Vector3 direction, float speed)
        {
            Position = new Vector3ReactiveProperty(position);
            Direction = direction;
            Speed = speed;
        }

        public bool Move(float dt)
        {
            // Move projectile
            Position.Value += dt * Speed * Direction;

            // Inidicate whether the projectile is out of level bounds or not
            return _levelConfig.IsPosOutOfVerticalBounds(Position.Value);
        }
    }
}
