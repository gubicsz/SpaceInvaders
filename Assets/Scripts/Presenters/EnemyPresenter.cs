using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class EnemyPresenter : MonoBehaviour
    {
        [SerializeField] GameObject[] _types;

        [Inject] EnemyModel _enemy;
        [Inject] EnemySpawner _enemySpawner;
        [Inject] EnemyConfig _enemyConfig;
        [Inject] ProjectileSpawner _projectileSpawner;
        [Inject] GameplayModel _gameplay;

        public void Init(int type, int row, int col)
        {
            // Initialize model
            _enemy.Init(type, row, col);

            // Update position based on model
            _enemy.Position.Subscribe(pos => transform.position = pos).AddTo(this);

            // Update enemy type based on model
            for (int i = 0; i < _types.Length; i++)
            {
                _types[i].SetActive(i == _enemy.Type);
            }

            // Handle projectile hit
            this.OnTriggerEnterAsObservable().Subscribe(collider =>
            {
                if (collider.TryGetComponent(out ProjectilePresenter projectile))
                {
                    // Despawn projectile
                    _projectileSpawner.Despawn(projectile);

                    // Despawn self
                    _enemySpawner.Despawn(this);

                    // Increase score by enemy type
                    _gameplay.CurrentScore.Value += (type + 1) * _enemyConfig.BaseScore;
                }
            }).AddTo(this);
        }

        public class Factory : PlaceholderFactory<Object, EnemyPresenter>
        {
        }
    }
}
