using UnityEngine;

namespace SpaceInvaders.Models
{
    public class ProjectileModel
    {
        public Vector3 Position { get; private set; }
        public Vector3 Direction { get; private set; }
        public float Speed { get; private set; }

        private readonly LevelConfig _levelConfig;

        public ProjectileModel(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
        }

        public void Init(Vector3 position, Vector3 direction, float speed)
        {
            // Set properties
            Position = position;
            Direction = direction;
            Speed = speed;
        }

        public void Reset()
        {
            // Reset properties
            Position = Vector3.zero;
            Direction = Vector3.zero;
            Speed = 0f;
        }

        public bool Move(float dt)
        {
            // Move projectile
            Position += dt * Speed * Direction;

            // Inidicate whether the projectile is out of level bounds or not
            return _levelConfig.IsPosOutOfVerticalBounds(Position);
        }
    }
}
