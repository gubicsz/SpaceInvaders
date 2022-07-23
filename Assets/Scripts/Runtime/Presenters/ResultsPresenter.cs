using SpaceInvaders.Helpers;
using SpaceInvaders.Models;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class ResultsPresenter : MonoBehaviour
    {
        [SerializeField] Button _buttonBack;
        [SerializeField] TextMeshProUGUI _labelScore;
        [SerializeField] TextMeshProUGUI _labelWaves;

        [Inject] readonly GameStateModel _gameState;
        [Inject] readonly GameplayModel _gameplay;
        [Inject] readonly ScoresModel _scores;

        private void Start()
        {
            // Return to main menu
            _buttonBack.OnClickAsObservable()
                .Subscribe(_ => _gameState.State.Value = GameState.Menu)
                .AddTo(this);

            // Update labels based on model
            _gameplay.CurrentScore.SubscribeToText(_labelScore).AddTo(this);
            _gameplay.CurrentWave.SubscribeToText(_labelWaves, (wave) => (wave - 1).ToString()).AddTo(this);

            // Add score to the scoreboard if it's greater than zero
            _gameState.State.Where(state => state == GameState.Results && _gameplay.CurrentScore.Value > 0)
                .Subscribe(_ => AddAndSaveScore()).AddTo(this);
        }

        private void AddAndSaveScore()
        {
            // Add new score item
            _scores.Add(new ScoreItem()
            {
                Score = _gameplay.CurrentScore.Value,
                Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
            });

            // Save scores to the device
            _scores.Save();
        }
    }
}
