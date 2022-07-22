using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpaceInvaders.Models;
using SpaceInvaders.Services;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class LoadingPresenter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _labelLoading;

        [Inject] readonly IAssetService _assetService;
        [Inject] readonly GameStateModel _gameState;
        [Inject] readonly ScoresModel _scores;

        readonly string[] _loadingTexts = new string[4]
        {
            "Loading", "Loading.", "Loading..", "Loading..."
        };

        private async void Start()
        {
            // Animate loading text
            Observable.Timer(TimeSpan.FromSeconds(0.25f), TimeSpan.FromSeconds(0.25))
                .Where(_ => gameObject.activeSelf)
                .Subscribe(tick => _labelLoading.text = _loadingTexts[tick % _loadingTexts.Length])
                .AddTo(this);

            // Initialize 
            DOTween.Init();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // Load scores
            _scores.Load();

            // Load assets
            await LoadAssetsAsync();

            // Load game scene
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).ToUniTask();

            // Proceed to the main menu
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
