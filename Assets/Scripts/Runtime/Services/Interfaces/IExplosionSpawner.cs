using SpaceInvaders.Presenters;
using UnityEngine;

namespace SpaceInvaders.Services
{
    public interface IExplosionSpawner
    {
        void Despawn(ExplosionPresenter explosion);
        void DespawnAll();
        void Spawn(Vector3 position);
    }
}
