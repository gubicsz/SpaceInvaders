using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders
{
    public class ScoresController : MonoBehaviour
    {
        [SerializeField]
        Button _buttonBack;

        [Inject]
        GameStateModel _gameState;

        [Inject]
        ScoresModel _scores;

        private void Start()
        {
            // Return to main menu
            _buttonBack.OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Menu)
                .AddTo(this);

            // todo: scores...
        }
    }
}
