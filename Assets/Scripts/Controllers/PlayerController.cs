using UnityEngine;
using Zenject;
using UniRx;

namespace SpaceInvaders
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] GameObject _shield;

        [Inject] PlayerModel _player;
        [Inject] InputService _input;
        [Inject] ProjectileSpawner _projectileSpawner;

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
                    _projectileSpawner.Spawn(_player.Position.Value + Vector3.forward, Vector3.forward);
                }
            }).AddTo(this);
        }

        public class Factory : PlaceholderFactory<Object, PlayerController>
        {
        }
    }
}
