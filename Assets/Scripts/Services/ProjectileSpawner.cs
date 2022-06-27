using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class ProjectileSpawner
    {
        private ProjectilePresenter.Factory _factory;
        private IAssetService _assetService;

        private List<ProjectilePresenter> _projectiles = new List<ProjectilePresenter>();

        public ProjectileSpawner(ProjectilePresenter.Factory factory, IAssetService assetService)
        {
            // Set references
            _factory = factory;
            _assetService = assetService;
        }

        public void Spawn(Vector3 position, Vector3 direction, float speed)
        {
            // Try to get projectile prefab
            var prefab = _assetService.Get<GameObject>("Projectile");

            // Handle error
            if (prefab == null)
            {
                return;
            }

            // Spawn projectile
            ProjectilePresenter projectile = _factory.Create(prefab);
            projectile.Init(position, direction, speed);
            _projectiles.Add(projectile);
        }

        public void Despawn(ProjectilePresenter projectile)
        {
            // Handle error
            if (projectile == null || !_projectiles.Contains(projectile))
            {
                return;
            }

            // Despawn projectile
            _projectiles.Remove(projectile);
            Object.Destroy(projectile.gameObject);
        }

        public void DespawnAll()
        {
            // Despawn projectiles
            foreach (var projectile in _projectiles)
            {
                Object.Destroy(projectile.gameObject);
            }

            // Clear list
            _projectiles.Clear();
        }
    }
}
