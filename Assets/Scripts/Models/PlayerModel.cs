using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UniRx;

namespace SpaceInvaders
{
    [Serializable]
    public class PlayerConfig
    {
        public int Lives = 3;
        public float Invulnerability = 3.0f;
        //todo: speed
    }

    public class PlayerModel : DisposableEntity
    {
        public ReactiveProperty<int> Lives { get; private set; }
        public ReactiveProperty<bool> IsInvulnerable { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsDead { get; private set; }
        public ReactiveCommand DamageCommand { get; private set; }
        // todo: position + level bounds + shoot

        private PlayerConfig _config;

        public PlayerModel(PlayerConfig config)
        {
            // Set references
            _config = config;

            // Set initial state
            Lives = new ReactiveProperty<int>(_config.Lives);
            IsInvulnerable = new ReactiveProperty<bool>(false);

            // The player is dead when he is out of lives
            IsDead = Lives.Select(lives => lives <= 0).ToReadOnlyReactiveProperty();

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
                await UniTask.Delay(TimeSpan.FromSeconds(_config.Invulnerability));
                IsInvulnerable.Value = false;

                //Observable.Timer(TimeSpan.FromSeconds(_config.Invulnerability))
                //    .Subscribe(_ => IsInvulnerable.Value = false).AddTo(this);
            }).AddTo(this);
        }
    }
}
