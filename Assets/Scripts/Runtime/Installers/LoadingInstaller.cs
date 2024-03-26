using Zenject;

namespace SpaceInvaders.Installers
{
    public class LoadingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // This is added only to trigger Project Context install first for the Loading scene.
        }
    }
}
