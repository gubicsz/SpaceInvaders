using Cysharp.Threading.Tasks;
using SpaceInvaders.Models;
using SpaceInvaders.Services;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] Light _light;
        [SerializeField] ParticleSystem _shield;
        [SerializeField] ParticleSystem _muzzleFlash;
        [SerializeField] ParticleSystem _thrusterRight;
        [SerializeField] ParticleSystem _thrusterLeft;

        [Inject] readonly PlayerModel _player;
        [Inject] readonly PlayerConfig _playerConfig;
        [Inject] readonly AudioConfig _audioConfig;
        [Inject] readonly InputModel _input;
        [Inject] readonly ProjectileSpawner _projectileSpawner;
        [Inject] readonly GameStateModel _gameState;
        [Inject] readonly CameraShaker _cameraShaker;
        [Inject] readonly IAudioService _audioService;

        private void Start()
        {
            // Reset player model
            _player.Reset();

            // Update transform based on model position
            _player.Position.Subscribe(pos => transform.position = pos).AddTo(this);

            // Set shield state based on invulnerability
            _player.IsInvulnerable.Subscribe(ManageShield).AddTo(this);

            // Control player
            Observable.EveryUpdate().Subscribe(_ =>
            {
                Move();
                Shoot();
            }).AddTo(this);

            // Handle projectile hit
            this.OnTriggerEnterAsObservable().Subscribe(collider =>
            {
                if (collider.TryGetComponent(out ProjectilePresenter projectile))
                {
                    OnHit(projectile);
                }
            }).AddTo(this);
        }

        private void Move()
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
        }

        private void Shoot()
        {
            // Shoot based on fire input
            if (_input.Fire && _player.Shoot(Time.time))
            {
                // Spawn projectile
                Vector3 spawnPos = _player.Position.Value + Vector3.forward * 1.5f;
                _projectileSpawner.Spawn(spawnPos, Vector3.forward, _playerConfig.ProjectileSpeed);

                // Play sfx and vfx
                _audioService.PlaySfx(Constants.Audio.Blaster, 0.25f);
                FlashLight();
            }
        }

        private void OnHit(ProjectilePresenter projectile)
        {
            // Play sfx and camera shake
            _audioService.PlaySfx(Constants.Audio.Explosion, 0.15f);

            if (!_player.IsInvulnerable.Value)
            {
                _audioService.DuckMusic(0.02f, _audioConfig.MusicVolume, 0.5f);
                _cameraShaker.Shake(0.5f, 1.0f);
            }

            // Damage player
            _projectileSpawner.Despawn(projectile);
            _player.DamageAsync().Forget();

            // End game when the player is out of lives
            if (_player.Lives.Value == 0)
            {
                _gameState.State.Value = GameState.Results;
            }
        }

        private void ManageShield(bool isInvulnerable)
        {
            if (isInvulnerable && !_shield.isPlaying)
            {
                _shield.Play();
            }
            else if (!isInvulnerable && _shield.isPlaying)
            {
                _shield.Stop();
            }
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

        private void FlashLight()
        {
            // Enable light for 100 ms
            Observable.Timer(TimeSpan.FromMilliseconds(100))
                .DoOnSubscribe(() => _light.enabled = true)
                .Subscribe(_ => _light.enabled = false).AddTo(this);

            _muzzleFlash.Play(true);
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, PlayerPresenter>
        {
        }
    }
}
