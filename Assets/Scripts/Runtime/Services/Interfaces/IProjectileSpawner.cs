using SpaceInvaders.Presenters;
using UnityEngine;

namespace SpaceInvaders.Services
{
    public interface IProjectileSpawner
    {
        void Despawn(ProjectilePresenter projectile);
        void DespawnAll();
        void Spawn(Vector3 position, Vector3 direction, float speed);
    }
}
