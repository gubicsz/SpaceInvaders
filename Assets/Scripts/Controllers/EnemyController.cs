using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] GameObject[] _types;

        [Inject] EnemyModel _enemy;

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
        }

        public class Factory : PlaceholderFactory<Object, EnemyController>
        {
        }
    }
}
