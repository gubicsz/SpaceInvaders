using UnityEngine;

namespace SpaceInvaders
{
    public class PlayerSpawner
    {
        private PlayerPresenter.Factory _factory;
        private PlayerPresenter _player;
        private IAssetService _assetService;

        public PlayerSpawner(PlayerPresenter.Factory factory, IAssetService assetService)
        {
            _factory = factory;
            _assetService = assetService;
        }

        public void Spawn()
        {
            // Try to get player prefab
            var prefab = _assetService.Get<GameObject>(Constants.Objects.Player);

            // Handle error
            if (prefab == null)
            {
                return;
            }

            // Spawn player
            _player = _factory.Create(prefab);
        }

        public void Despawn()
        {
            // The player hasn't been spawned yet
            if (_player == null)
            {
                return;
            }

            // Despawn player
            Object.Destroy(_player.gameObject);
            _player = null;
        }
    }
}