using System;
using System.Linq;
using UniRx;

namespace SpaceInvaders
{
    public class PlayerModel : DisposableEntity
    {
        public ReactiveProperty<int> Lives { get; private set; }
        public ReactiveProperty<bool> IsInvulnerable { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsDead { get; private set; }
        public ReactiveCommand DamageCommand { get; private set; }
        // todo: position + level bounds + shoot

        private IDisposable _timer;

        public PlayerModel(PlayerConfig playerConfig)
        {
            // Set initial state
            Lives = new ReactiveProperty<int>(playerConfig.Lives);
            IsInvulnerable = new ReactiveProperty<bool>(false);

            // The player is dead when he is out of lives
            IsDead = Lives.Select(lives => lives <= 0).ToReadOnlyReactiveProperty();

            // The player can only be damaged when not dead yet
            DamageCommand = IsDead.Select(isDead => !isDead).ToReactiveCommand();

            // Handle player hit
            DamageCommand.Subscribe(_ =>
            {
                // Reduce lives and start invulnerability
                Lives.Value--;
                IsInvulnerable.Value = true;

                // Stop the previous timer
                _timer?.Dispose();

                // Stop invulnerability in X seconds
                _timer = Observable
                    .Timer(TimeSpan.FromSeconds(playerConfig.Invulnerability))
                    .Subscribe(_ => IsInvulnerable.Value = false);
            }).AddTo(this);
        }
    }
}
