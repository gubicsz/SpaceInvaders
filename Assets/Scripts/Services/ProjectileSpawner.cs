using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class ProjectileSpawner
    {
        private ProjectileController.Factory _factory;
        private AddressablesService _addressables;

        private List<ProjectileController> _projectiles = new List<ProjectileController>();

        public ProjectileSpawner(ProjectileController.Factory factory, AddressablesService addressables)
        {
            // Set references
            _factory = factory;
            _addressables = addressables;
        }

        public void Spawn(Vector3 position, Vector3 direction)
        {
            // Try to get projectile prefab
            var prefab = _addressables.GetGameObject("Projectile");

            // Handle error
            if (prefab == null)
            {
                return;
            }

            // Spawn projectile
            ProjectileController projectile = _factory.Create(prefab);
            projectile.Init(position, direction);
            _projectiles.Add(projectile);
        }

        public void Despawn(ProjectileController projectile)
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
