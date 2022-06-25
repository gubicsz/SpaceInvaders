using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class PlayerConfig
    {
        public int Lives = 3;
        public float Invulnerability = 3.0f;
        public float Speed = 5.0f;
        public float FireRate = 0.5f;
        public Vector3 SpawnPosition = new Vector3(0f, 0f, -10f);
    }

    [Serializable]
    public class LevelConfig
    {
        public Vector3 Bounds = new Vector3(20f, 0f, 11f);

        public bool IsPosOutOfHorizontalBounds(Vector3 pos)
        {
            return (pos.x > Bounds.x) || (pos.x < -Bounds.x);
        }

        public bool IsPosOutOfVerticalBounds(Vector3 pos)
        {
            return (pos.z > Bounds.z) || (pos.z < -Bounds.z);
        }
    }

    public class PlayerModel : DisposableEntity
    {
        public Vector3ReactiveProperty Position { get; private set; }
        public ReactiveProperty<int> Lives { get; private set; }
        public ReactiveProperty<bool> IsInvulnerable { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsDead { get; private set; }
        public ReactiveCommand DamageCommand { get; private set; }//todo: refactor

        private PlayerConfig _playerConfig;
        private LevelConfig _levelConfig;
        private float _lastShot;

        public PlayerModel(PlayerConfig playerConfig, LevelConfig levelConfig)
        {
            // Set references
            _playerConfig = playerConfig;
            _levelConfig = levelConfig;

            // Set initial state
            Position = new Vector3ReactiveProperty(_playerConfig.SpawnPosition);
            Lives = new ReactiveProperty<int>(_playerConfig.Lives);
            IsInvulnerable = new ReactiveProperty<bool>(false);

            // The player is dead when he is out of lives
            IsDead = Lives.Select(lives => lives <= 0).ToReadOnlyReactiveProperty();

            // TODO: refactor damage
            // The player can only be damaged when he is alive and vulnerable
            DamageCommand = IsDead.CombineLatest(IsInvulnerable, 
                (isDead, isInvulnerable) => !isDead && !isInvulnerable).ToReactiveCommand();

            // Handle player hit
            DamageCommand.Subscribe(async _ =>
            {
                // Reduce lives and start invulnerability
                Lives.Value--;
                IsInvulnerable.Value = true;

                // Stop invulnerability in X seconds
                await UniTask.Delay(TimeSpan.FromSeconds(_playerConfig.Invulnerability));
                IsInvulnerable.Value = false;

                //Observable.Timer(TimeSpan.FromSeconds(_config.Invulnerability))
                //    .Subscribe(_ => IsInvulnerable.Value = false).AddTo(this);//todo
            }).AddTo(this);
        }

        public void Reset()
        {
            // Reset values
            Position.Value = _playerConfig.SpawnPosition;
            Lives.Value = _playerConfig.Lives;
            IsInvulnerable.Value = false;
            _lastShot = 0f;
        }

        public void Move(float horizontal, float dt)
        {
            // Calculate delta position
            Vector3 deltaPos = horizontal * dt * _playerConfig.Speed * Vector3.right;

            // Prevent moving out of level bounds
            if (_levelConfig.IsPosOutOfHorizontalBounds(Position.Value + deltaPos))
            {
                return;
            }

            // Increment position
            Position.Value += deltaPos;
        }
        
        public bool Shoot(float currentTime)
        {
            // Prevent shooting based on fire rate
            if (currentTime < (_lastShot + _playerConfig.FireRate))
            {
                return false;
            }

            // Register last shot time
            _lastShot = currentTime;
            return true;
        }

        public void Damage()
        {
            // TODO
        }
    }
}
