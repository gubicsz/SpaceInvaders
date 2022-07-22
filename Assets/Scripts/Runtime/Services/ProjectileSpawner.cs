using SpaceInvaders.Presenters;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Services
{
    public class ProjectileSpawner
    {
        readonly ProjectilePresenter.Factory _factory;

        readonly List<ProjectilePresenter> _projectiles = new();

        public ProjectileSpawner(ProjectilePresenter.Factory factory)
        {
            _factory = factory;
        }

        public void Spawn(Vector3 position, Vector3 direction, float speed)
        {
            // Spawn projectile
            ProjectilePresenter projectile = _factory.Create(position, direction, speed);
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
            projectile.Dispose();
            _projectiles.Remove(projectile);
        }

        public void DespawnAll()
        {
            // Despawn projectiles
            foreach (var projectile in _projectiles)
            {
                projectile.Dispose();
            }

            // Clear list
            _projectiles.Clear();
        }
    }
}
