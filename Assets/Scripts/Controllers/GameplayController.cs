using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField]
        Button _buttonExit;

        [SerializeField]
        TextMeshProUGUI _labelScore;

        [SerializeField]
        TextMeshProUGUI _labelWave;

        [SerializeField]
        TextMeshProUGUI _labelLives;

        [Inject]
        GameStateModel _gameState;

        [Inject]
        GameplayModel _gameplay;

        [Inject]
        PlayerModel _player;

        private void Start()
        {
            // Exit to main menu
            _buttonExit.OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Menu)
                .AddTo(this);

            // Update labels based on models
            _gameplay.CurrentScore.SubscribeToText(_labelScore).AddTo(this);
            _gameplay.CurrentWave.SubscribeToText(_labelWave, (wave) => (wave + 1).ToString()).AddTo(this);
            _player.Lives.SubscribeToText(_labelLives).AddTo(this);
        }
    }
}
