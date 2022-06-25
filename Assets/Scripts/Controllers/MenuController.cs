using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] Button _buttonStart;
        [SerializeField] Button _buttonScores;

        [Inject] GameStateModel _gameState;

        private void Start()
        {
            // Proceed to gameplay
            _buttonStart.OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Gameplay)
                .AddTo(this);

            // Proceed to high scores
            _buttonScores.OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Scores)
                .AddTo(this);
        }
    }
}
