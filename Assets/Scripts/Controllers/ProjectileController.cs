using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class ProjectileController : MonoBehaviour
    {
        [Inject] ProjectileModel _projectile;
        [Inject] ProjectileSpawner _projectileSpawner;

        public void Init(Vector3 position, Vector3 direction)
        {
            // Init model
            _projectile.Init(position, direction);

            // Update position based on model
            _projectile.Position.Subscribe(pos => transform.position = pos).AddTo(this);

            // Update model
            Observable.EveryUpdate().Subscribe(_ =>
            {
                // Despawn projectile when it out of level bounds
                if (_projectile.Move(Time.deltaTime))
                {
                    _projectileSpawner.Despawn(this);
                }
            }).AddTo(this);
        }

        public class Factory : PlaceholderFactory<Object, ProjectileController>
        {
        }
    }
}
