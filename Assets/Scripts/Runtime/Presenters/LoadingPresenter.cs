using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpaceInvaders.Models;
using SpaceInvaders.Services;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class LoadingPresenter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _labelLoading;

        [Inject]
        private readonly IAssetService _assetService;

        [Inject]
        private readonly GameStateModel _gameState;

        private readonly string[] _loadingTexts = new string[4]
        {
            "Loading",
            "Loading.",
            "Loading..",
            "Loading..."
        };

        [Inject]
        private readonly ScoresModel _scores;

        private async void Start()
        {
            // Animate loading text
            Observable
                .Timer(TimeSpan.FromSeconds(0.25f), TimeSpan.FromSeconds(0.25))
                .Where(_ => gameObject.activeSelf)
                .Subscribe(tick => _labelLoading.text = _loadingTexts[tick % _loadingTexts.Length])
                .AddTo(this);

            // Initialize
            DOTween.Init();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            _scores.Load();

            // Load assets and game
            await LoadAssetsAsync();
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).ToUniTask();
            _gameState.State.Value = GameState.Menu;
        }

        private async UniTask LoadAssetsAsync()
        {
            // Load objects
            await _assetService.Load<GameObject>(Constants.Objects.Enemy);
            await _assetService.Load<GameObject>(Constants.Objects.Player);
            await _assetService.Load<GameObject>(Constants.Objects.Projectile);
            await _assetService.Load<GameObject>(Constants.Objects.Blast);

            // Load audio
            await _assetService.Load<AudioClip>(Constants.Audio.Blaster);
            await _assetService.Load<AudioClip>(Constants.Audio.Explosion);
            await _assetService.Load<AudioClip>(Constants.Audio.Click);
            await _assetService.Load<AudioClip>(Constants.Audio.Music);

            // Load materials
            await _assetService.Load<Material>(Constants.Materials.Background);
        }
    }
}
