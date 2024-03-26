using SpaceInvaders.Models;
using SpaceInvaders.Services;
using Zenject;

namespace SpaceInvaders.Installers
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
