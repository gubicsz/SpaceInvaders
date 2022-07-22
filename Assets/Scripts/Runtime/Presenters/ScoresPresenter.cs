using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders
{
    public class ScoresPresenter : MonoBehaviour
    {
        [SerializeField] Button _buttonBack;
        [SerializeField] TextMeshProUGUI _itemPrefab;
        [SerializeField] VerticalLayoutGroup _itemHolder;

        [Inject] GameStateModel _gameState;
        [Inject] ScoresModel _scores;

        private BoundItemsContainer<ScoreItem> _scoresContainer;

        private void Start()
        {
            // Return to main menu
            _buttonBack.OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Menu)
                .AddTo(this);

            // Create score items container
            _scoresContainer = new BoundItemsContainer<ScoreItem>(_itemPrefab.gameObject, _itemHolder.gameObject)
            {
                DestroyOnRemove = true
            };

            // Handle add event
            _scoresContainer.ObserveAdd().Subscribe(e =>
            {
                if (e.GameObject.TryGetComponent(out TextMeshProUGUI item))
                {
                    item.text = $"<color=#00ffff>{e.Model.Score}</color> - {e.Model.Date}";
                }
            }).AddTo(this);

            // Initialize container
            _scoresContainer.Initialize(_scores.Scoreboard);
            _scoresContainer.AddTo(this);
        }
    }
}
