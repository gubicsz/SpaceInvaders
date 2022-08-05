using SpaceInvaders.Presenters;
using System.Collections.Generic;

namespace SpaceInvaders.Services
{
    public interface IEnemySpawner
    {
        List<EnemyPresenter> Enemies { get; }

        void Despawn(EnemyPresenter enemy);
        void DespawnAll();
        void SpawnAll();
    }
}