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

            // Services
            Container.BindInterfacesAndSelfTo<AddressablesService>().AsSingle();
            Container.BindInterfacesAndSelfTo<StorageService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();

            // Player
            Container.Bind<PlayerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
            Container.BindFactory<Object, PlayerController, PlayerController.Factory>().FromFactory<PrefabFactory<PlayerController>>();

            // Enemies
            Container.Bind<EnemyModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyMover>().AsSingle();
            Container.BindFactory<Object, EnemyController, EnemyController.Factory>().FromFactory<PrefabFactory<EnemyController>>();

            // Projectiles
            Container.Bind<ProjectileModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsSingle();
            // todo: mover
            Container.BindFactory<Object, ProjectileController, ProjectileController.Factory>().FromFactory<PrefabFactory<ProjectileController>>();

            // TODO
            //Container.BindAsync<GameObject>().FromMethod(async () =>
            //{
            //    try
            //    {
            //        var addressables = Container.Resolve<AddressablesService>();
            //        var locations = await Addressables.LoadResourceLocationsAsync("Player").ToUniTask();
            //        var go = await Addressables.LoadAssetAsync<GameObject>(locations[0]).ToUniTask();
            //        //go = Container.InstantiatePrefab(go);
            //        //var go = await Addressables.InstantiateAsync(locations[0]).ToUniTask();
            //        return go;
            //    }
            //    catch (InvalidKeyException)
            //    {

            //    }
            //    return null;
            //}).AsCached();
        }
    }
}
