using Zenject;

namespace SpaceInvaders
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Global
            Container.Bind<GameStateModel>().AsSingle();
            Container.Bind<ScoresModel>().AsSingle();

            // Services
            Container.BindInterfacesAndSelfTo<StorageService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressablesService>().AsSingle();
        }
    }
}