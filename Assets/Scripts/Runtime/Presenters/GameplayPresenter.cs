using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpaceInvaders.Helpers;
using SpaceInvaders.Models;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class GameplayPresenter : MonoBehaviour
    {
        [SerializeField] Button _buttonExit;
        [SerializeField] TextMeshProUGUI _labelScore;
        [SerializeField] TextMeshProUGUI _labelWave;
        [SerializeField] TextMeshProUGUI _labelLives;
        [SerializeField] Image _imageVignette;

        [Inject] readonly GameStateModel _gameState;
        [Inject] readonly GameplayModel _gameplay;
        [Inject] readonly PlayerModel _player;

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
            _player.Lives.Pairwise().Where(lives => lives.Current < lives.Previous).Subscribe(_ =>
            {
                _imageVignette.DOFade(1f, 0.25f)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.OutQuint)
                    .OnStart(() => _imageVignette.color = new Color(1f, 1f, 1f, 0f));
            }).AddTo(this);

            // Animate score label on increase
            _gameplay.CurrentScore.Pairwise().Where(score => score.Current > score.Previous).Subscribe(_ =>
            {
                _labelScore.rectTransform
                    .DOPunchScale(new Vector3(0.25f, 0.25f, 0f), 0.125f)
                    .SetEase(Ease.OutQuint)
                    .OnComplete(() => _labelScore.rectTransform.localScale = Vector3.one);
            }).AddTo(this);
        }
    }
}
