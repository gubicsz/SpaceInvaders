using System.Collections.Generic;
using SpaceInvaders.Presenters;

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
