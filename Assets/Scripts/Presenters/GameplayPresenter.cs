using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders
{
    public class GameplayPresenter : MonoBehaviour
    {
        [SerializeField] Button _buttonExit;
        [SerializeField] TextMeshProUGUI _labelScore;
        [SerializeField] TextMeshProUGUI _labelWave;
        [SerializeField] TextMeshProUGUI _labelLives;
        [SerializeField] Image _imageVignette;

        [Inject] GameStateModel _gameState;
        [Inject] GameplayModel _gameplay;
        [Inject] PlayerModel _player;

        private void Start()
        {
            // Exit to main menu
            _buttonExit.OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Menu)
                .AddTo(this);

            // Reset model when the gameplay starts
            _gameState.State.Where(state => state == GameState.Gameplay)
                .Subscribe(_ => _gameplay.Reset()).AddTo(this);

            // Update labels based on models
            _gameplay.CurrentScore.SubscribeToText(_labelScore).AddTo(this);
            _gameplay.CurrentWave.SubscribeToText(_labelWave).AddTo(this);
            _player.Lives.SubscribeToText(_labelLives).AddTo(this);

            // Show screen hit effect when the player is damaged
            _player.Lives.Pairwise().Where(lives => lives.Current < lives.Previous).Subscribe(async _ =>
            {
                _imageVignette.enabled = true;
                await UniTask.Delay(250);
                _imageVignette.enabled = false;
            }).AddTo(this);
        }
    }
}
