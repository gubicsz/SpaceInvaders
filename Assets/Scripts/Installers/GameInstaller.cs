using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Game
            Container.Bind<GameStateModel>().AsSingle();
            Container.Bind<GameplayModel>().AsSingle();
            Container.Bind<ScoresModel>().AsSingle();
            Container.Bind<InputModel>().AsSingle();

            // Services
            Container.BindInterfacesAndSelfTo<AddressablesService>().AsSingle();
            Container.BindInterfacesAndSelfTo<StorageService>().AsSingle();

            // Player
            Container.Bind<PlayerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
            Container.BindFactory<Object, PlayerPresenter, PlayerPresenter.Factory>().FromFactory<PrefabFactory<PlayerPresenter>>();

            // Enemies
            Container.Bind<EnemyModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesManager>().AsSingle();
            Container.BindFactory<Object, EnemyPresenter, EnemyPresenter.Factory>().FromFactory<PrefabFactory<EnemyPresenter>>();

            // Projectiles
            Container.Bind<ProjectileModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsSingle();
            Container.BindFactory<Object, ProjectilePresenter, ProjectilePresenter.Factory>().FromFactory<PrefabFactory<ProjectilePresenter>>();
        }
    }
}
