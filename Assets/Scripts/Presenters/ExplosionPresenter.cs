using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class ExplosionPresenter : MonoBehaviour, IPoolable<Vector3, IMemoryPool>, IDisposable
    {
        [SerializeField] ParticleSystem _particle;

        [Inject] ExplosionSpawner _particleSpawner;

        IMemoryPool _pool;

        public async void OnSpawned(Vector3 position, IMemoryPool pool)
        {
            // Init
            _pool = pool;
            transform.position = position;
            _particle.Play();

            // Despawn particle after the duration
            await UniTask.Delay(Mathf.RoundToInt(_particle.main.duration * 1000));
            _particleSpawner.Despawn(this);
        }

        public void OnDespawned()
        {
            // Reset
            _pool = null;
            _particle.Stop();
            transform.position = Vector3.zero;
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Vector3, ExplosionPresenter>
        {
        }
    }
}
