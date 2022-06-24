using Zenject;

namespace SpaceInvaders
{
    public class EnemySpawner
    {
        private EnemyModel.Factory _spawner;
        private AssetLoader _loader;

        [Inject]
        public EnemySpawner(EnemyModel.Factory spawner, AssetLoader loader)
        {
            _spawner = spawner;
            _loader = loader;
        }

        public EnemyModel Spawn()
        {
            return _spawner.Create(_loader.GetAsset("Enemy"));
        }
    }
}
