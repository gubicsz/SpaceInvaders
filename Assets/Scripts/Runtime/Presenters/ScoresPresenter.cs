using SpaceInvaders.Helpers;
using SpaceInvaders.Models;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class ScoresPresenter : MonoBehaviour
    {
        [SerializeField]
        private Button _buttonBack;

        [SerializeField]
        private TextMeshProUGUI _itemPrefab;

        [SerializeField]
        private VerticalLayoutGroup _itemHolder;

        [Inject]
        private readonly GameStateModel _gameState;

        [Inject]
        private readonly ScoresModel _scores;

        private BoundItemsContainer<ScoreItem> _scoresContainer;

        private void Start()
        {
            // Return to main menu
            _buttonBack
                .OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Menu)
                .AddTo(this);

            // Create score items container
            _scoresContainer = new BoundItemsContainer<ScoreItem>(
                _itemPrefab.gameObject,
                _itemHolder.gameObject
            )
            {
                DestroyOnRemove = true
            };

            // Handle add event
            _scoresContainer
                .ObserveAdd()
                .Subscribe(e =>
                {
                    if (e.GameObject.TryGetComponent(out TextMeshProUGUI item))
                        item.text = $"<color=#00ffff>{e.Model.Score}</color> - {e.Model.Date}";
                })
                .AddTo(this);

            // Initialize container
            _scoresContainer.Initialize(_scores.Scoreboard);
            _scoresContainer.AddTo(this);
        }
    }
}
