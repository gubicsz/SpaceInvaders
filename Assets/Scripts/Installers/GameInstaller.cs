using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameModel>().AsSingle().NonLazy();
            Container.Bind<GameStateModel>().AsSingle();
            Container.Bind<ScoresModel>().AsSingle();
            Container.Bind<GameplayModel>().AsSingle();

            Container.Bind<PlayerModel>().AsSingle();//TODO: FACTORY?

            Container.BindInterfacesAndSelfTo<AssetLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<StorageService>().AsSingle();

            //Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            //Container.BindFactory<Object, EnemyModel, EnemyModel.Factory>().FromFactory<PrefabFactory<EnemyModel>>();
        }
    }
}
