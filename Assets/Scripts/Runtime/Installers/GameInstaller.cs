using SpaceInvaders.Models;
using SpaceInvaders.Presenters;
using SpaceInvaders.Services;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] readonly IAssetService _assetService;

        public override void InstallBindings()
        {
            // Game
            Container.Bind<GameplayModel>().AsSingle();
            Container.Bind<InputModel>().AsSingle();

            // Services
            Container.BindInterfacesAndSelfTo<StorageService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AudioService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraShaker>().AsSingle();

            // Player
            Container.Bind<PlayerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
            Container.BindFactory<Object, PlayerPresenter, PlayerPresenter.Factory>().FromFactory<PrefabFactory<PlayerPresenter>>();

            // Enemies
            Container.Bind<EnemyModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesManager>().AsSingle();
            Container.BindFactory<int, int, int, EnemyPresenter, EnemyPresenter.Factory>().FromMonoPoolableMemoryPool(
                x => x.WithInitialSize(64).FromComponentInNewPrefab(_assetService.Get<GameObject>(Constants.Objects.Enemy)).UnderTransformGroup("EnemyPool"));

            // Projectiles
            Container.Bind<ProjectileModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsSingle();
            Container.BindFactory<Vector3, Vector3, float, ProjectilePresenter, ProjectilePresenter.Factory>().FromMonoPoolableMemoryPool(
                x => x.WithInitialSize(8).FromComponentInNewPrefab(_assetService.Get<GameObject>(Constants.Objects.Projectile)).UnderTransformGroup("ProjectilePool"));

            // Explosions
            Container.BindInterfacesAndSelfTo<ExplosionSpawner>().AsSingle();
            Container.BindFactory<Vector3, ExplosionPresenter, ExplosionPresenter.Factory>().FromMonoPoolableMemoryPool(
                x => x.WithInitialSize(4).FromComponentInNewPrefab(_assetService.Get<GameObject>(Constants.Objects.Blast)).UnderTransformGroup("ExplosionPool"));
        }
    }
}
