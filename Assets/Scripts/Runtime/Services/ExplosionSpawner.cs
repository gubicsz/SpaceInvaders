using System.Collections.Generic;
using SpaceInvaders.Presenters;
using UnityEngine;

namespace SpaceInvaders.Services
{
    public class ExplosionSpawner : IExplosionSpawner
    {
        private readonly List<ExplosionPresenter> _explosions = new();
        private readonly ExplosionPresenter.Factory _factory;

        public ExplosionSpawner(ExplosionPresenter.Factory factory)
        {
            // Set references
            _factory = factory;
        }

        public void Spawn(Vector3 position)
        {
            // Spawn explosion
            var explosion = _factory.Create(position);
            _explosions.Add(explosion);
        }

        public void Despawn(ExplosionPresenter explosion)
        {
            // Handle error
            if (explosion == null || !_explosions.Contains(explosion))
                return;

            // Despawn explosion
            explosion.Dispose();
            _explosions.Remove(explosion);
        }

        public void DespawnAll()
        {
            // Despawn explosion
            for (var i = 0; i < _explosions.Count; i++)
                _explosions[i].Dispose();

            // Clear list
            _explosions.Clear();
        }
    }
}
