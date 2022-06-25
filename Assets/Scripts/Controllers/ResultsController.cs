using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders
{
    public class ResultsController : MonoBehaviour
    {
        [SerializeField] Button _buttonBack;
        [SerializeField] TextMeshProUGUI _labelScore;
        [SerializeField] TextMeshProUGUI _labelWaves;

        [Inject] GameStateModel _gameState;
        [Inject] GameplayModel _gameplay;

        private void Start()
        {
            // Return to main menu
            _buttonBack.OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Menu)
                .AddTo(this);

            // Update labels based on model
            _gameplay.CurrentScore.SubscribeToText(_labelScore).AddTo(this);
            _gameplay.CurrentWave.SubscribeToText(_labelWaves, (wave) => (wave - 1).ToString()).AddTo(this);
        }
    }
}
