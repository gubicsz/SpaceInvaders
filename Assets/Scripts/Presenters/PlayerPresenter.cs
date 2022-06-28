using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

namespace SpaceInvaders
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] GameObject _shield;

        [Inject] PlayerModel _player;
        [Inject] PlayerConfig _playerConfig;
        [Inject] InputModel _input;
        [Inject] ProjectileSpawner _projectileSpawner;
        [Inject] GameStateModel _gameState;
        [Inject] CameraShaker _cameraShaker;
        [Inject] IAudioService _audioService;

        private void Start()
        {
            // Reset player model
            _player.Reset();

            // Update transform based on model position
            _player.Position.Subscribe(pos => transform.position = pos).AddTo(this);

            // Set shield state based on invulnerability
            _player.IsInvulnerable.Subscribe(isInvulnerable => _shield.SetActive(isInvulnerable)).AddTo(this);

            // Control player
            Observable.EveryUpdate().Subscribe(_ =>
            {
                // Move based on horizontal input
                _player.Move(_input.Horizontal, Time.deltaTime);

                // Shoot based on fire input
                if (_input.Fire && _player.Shoot(Time.time))
                {
                    // Spawn projectile
                    Vector3 spawnPos = _player.Position.Value + Vector3.forward * 1.5f;
                    _projectileSpawner.Spawn(spawnPos, Vector3.forward, _playerConfig.ProjectileSpeed);

                    // Play blaster sfx
                    _audioService.PlaySfx(Constants.Audio.Blaster, 0.25f);
                }
            }).AddTo(this);

            // Handle projectile hit
            this.OnTriggerEnterAsObservable().Subscribe(collider =>
            {
                if (collider.TryGetComponent(out ProjectilePresenter projectile))
                {
                    // Play explosion sfx
                    _audioService.PlaySfx(Constants.Audio.Explosion, 0.15f);

                    // Shake camera:
                    _cameraShaker.Shake(0.25f, 0.25f);

                    // Despawn projectile
                    _projectileSpawner.Despawn(projectile);

                    // Damage player
                    _player.DamageAsync().Forget();

                    // End game when the player is out of lives
                    if (_player.Lives.Value == 0)
                    {
                        _gameState.State.Value = GameState.Results;
                    }
                }
            }).AddTo(this);
        }

        public class Factory : PlaceholderFactory<Object, PlayerPresenter>
        {
        }
    }
}
