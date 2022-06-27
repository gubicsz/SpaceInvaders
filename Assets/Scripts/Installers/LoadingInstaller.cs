using Zenject;

namespace SpaceInvaders
{
    public class LoadingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // This is added only to trigger Project Context install first for the Loading scene.
        }
    }
}