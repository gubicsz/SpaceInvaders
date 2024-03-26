using System;
using Cysharp.Threading.Tasks;
using SpaceInvaders.Helpers;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Models
{
    [Serializable]
    public class PlayerConfig
    {
        [Tooltip("Number of lives.")]
        public int Lives = 3;

        [Tooltip("Duration of invulnerability in seconds.")]
        public float Invulnerability = 3.0f;

        [Tooltip("Horizontal speed in m/s.")]
        public float Speed = 5.0f;

        [Tooltip("Time between player projectiles in seconds.")]
        public float FireRate = 0.5f;

        [Tooltip("Speed of a player projectile in m/s.")]
        public float ProjectileSpeed = 15.0f;

        [Tooltip("Position of the player at start.")]
        public Vector3 SpawnPosition = new(0f, 0f, -10f);
    }

    [Serializable]
    public class LevelConfig
    {
        public Vector3 Bounds = new(20f, 0f, 11f);

        public bool IsPosOutOfHorizontalBounds(Vector3 pos)
        {
            var refAspectRatio = 16f / 9;
            var currentAspectRatio = Screen.width / (float)Screen.height;
            var boundX = Bounds.x * (currentAspectRatio / refAspectRatio);

            return pos.x > boundX || pos.x < -boundX;
        }

        public bool IsPosOutOfVerticalBounds(Vector3 pos)
        {
            return pos.z > Bounds.z || pos.z < -Bounds.z;
        }
    }

    public class PlayerModel : DisposableEntity
    {
        private readonly LevelConfig _levelConfig;

        private readonly PlayerConfig _playerConfig;

        public PlayerModel(PlayerConfig playerConfig, LevelConfig levelConfig)
        {
            // Set references
            _playerConfig = playerConfig;
            _levelConfig = levelConfig;

            // Set initial state
            Position = new Vector3ReactiveProperty(_playerConfig.SpawnPosition);
            Lives = new ReactiveProperty<int>(_playerConfig.Lives);
            IsInvulnerable = new ReactiveProperty<bool>(false);
            ShotTime = float.MinValue;

            // The player is dead when he is out of lives
            IsDead = Lives.Select(lives => lives <= 0).ToReadOnlyReactiveProperty();
        }

        public Vector3ReactiveProperty Position { get; }
        public ReactiveProperty<int> Lives { get; }
        public ReactiveProperty<bool> IsInvulnerable { get; }
        public ReadOnlyReactiveProperty<bool> IsDead { get; }
        public float ShotTime { get; private set; }

        public void Reset()
        {
            // Reset values
            Position.Value = _playerConfig.SpawnPosition;
            Lives.Value = _playerConfig.Lives;
            IsInvulnerable.Value = false;
            ShotTime = float.MinValue;
        }

        public void Move(float horizontal, float dt)
        {
            // Calculate delta position
            var deltaPos = horizontal * dt * _playerConfig.Speed * Vector3.right;

            // Prevent moving out of level bounds
            if (_levelConfig.IsPosOutOfHorizontalBounds(Position.Value + deltaPos))
                return;

            // Increment position
            Position.Value += deltaPos;
        }

        public bool Shoot(float currentTime)
        {
            // Prevent shooting based on fire rate
            if (currentTime < ShotTime + _playerConfig.FireRate)
                return false;

            // Register last shot time
            ShotTime = currentTime;
            return true;
        }

        public async UniTask<bool> DamageAsync()
        {
            // Do nothing if the player is dead or invulnerable
            if (IsDead.Value || IsInvulnerable.Value)
                return false;

            // Reduce lives and start invulnerability
            Lives.Value--;
            IsInvulnerable.Value = true;

            // Stop invulnerability in X seconds
            await UniTask.Delay(TimeSpan.FromSeconds(_playerConfig.Invulnerability));
            IsInvulnerable.Value = false;

            return true;
        }
    }
}
