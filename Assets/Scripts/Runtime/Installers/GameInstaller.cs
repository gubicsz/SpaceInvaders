using SpaceInvaders.Models;
using SpaceInvaders.Presenters;
using SpaceInvaders.Services;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private readonly IAssetService _assetService;

        public override void InstallBindings()
        {
            InstallGame();
            InstallServices();
            InstallPlayer();
            InstallEnemies();
            InstallProjectiles();
            InstallExplosions();
        }

        private void InstallGame()
        {
            Container.Bind<GameplayModel>().AsSingle();
            Container.Bind<InputModel>().AsSingle();
        }

        private void InstallServices()
        {
            Container.BindInterfacesAndSelfTo<StorageService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AudioService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraShaker>().AsSingle();
        }

        private void InstallPlayer()
        {
            Container.Bind<PlayerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
            Container
                .BindFactory<Object, PlayerPresenter, PlayerPresenter.Factory>()
                .FromFactory<PrefabFactory<PlayerPresenter>>();
        }

        private void InstallEnemies()
        {
            Container.Bind<EnemyModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesManager>().AsSingle();
            Container
                .BindFactory<int, int, int, EnemyPresenter, EnemyPresenter.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.WithInitialSize(64)
                        .FromComponentInNewPrefab(
                            _assetService.Get<GameObject>(Constants.Objects.Enemy)
                        )
                        .UnderTransformGroup("EnemyPool")
                );
        }

        private void InstallProjectiles()
        {
            Container.Bind<ProjectileModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsSingle();
            Container
                .BindFactory<
                    Vector3,
                    Vector3,
                    float,
                    ProjectilePresenter,
                    ProjectilePresenter.Factory
                >()
                .FromMonoPoolableMemoryPool(x =>
                    x.WithInitialSize(8)
                        .FromComponentInNewPrefab(
                            _assetService.Get<GameObject>(Constants.Objects.Projectile)
                        )
                        .UnderTransformGroup("ProjectilePool")
                );
        }

        private void InstallExplosions()
        {
            Container.BindInterfacesAndSelfTo<ExplosionSpawner>().AsSingle();
            Container
                .BindFactory<Vector3, ExplosionPresenter, ExplosionPresenter.Factory>()
                .FromMonoPoolableMemoryPool(x =>
                    x.WithInitialSize(4)
                        .FromComponentInNewPrefab(
                            _assetService.Get<GameObject>(Constants.Objects.Blast)
                        )
                        .UnderTransformGroup("ExplosionPool")
                );
        }
    }
}
