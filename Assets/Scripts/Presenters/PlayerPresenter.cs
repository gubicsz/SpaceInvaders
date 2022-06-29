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
        [SerializeField] ParticleSystem _thrusterRight;
        [SerializeField] ParticleSystem _thrusterLeft;

        [Inject] PlayerModel _player;
        [Inject] PlayerConfig _playerConfig;
        [Inject] AudioConfig _audioConfig;
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

                // Manage thruster particles based on horizontal input
                if (_input.Horizontal > 0f)
                {
                    StopThruster(_thrusterRight);
                    StartThruster(_thrusterLeft);
                }
                else if (_input.Horizontal < 0f)
                {
                    StopThruster(_thrusterLeft);
                    StartThruster(_thrusterRight);
                }
                else
                {
                    StopThruster(_thrusterRight);
                    StopThruster(_thrusterLeft);
                }

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
                    // Duck music
                    _audioService.DuckMusic(0.02f, _audioConfig.MusicVolume, 0.5f);

                    // Play explosion sfx
                    _audioService.PlaySfx(Constants.Audio.Explosion, 0.15f);

                    // Shake camera:
                    _cameraShaker.Shake(0.5f, 1.0f);

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

        private void StartThruster(ParticleSystem particleSystem)
        {
            // Play the particle if needed
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }

        private void StopThruster(ParticleSystem particleSystem)
        {
            // Stop the particle if needed
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        public class Factory : PlaceholderFactory<Object, PlayerPresenter>
        {
        }
    }
}
