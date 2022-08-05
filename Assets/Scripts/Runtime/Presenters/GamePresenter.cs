using SpaceInvaders.Models;
using SpaceInvaders.Services;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] MeshRenderer _background;

        [Inject] readonly IEnemiesManager _enemiesManager;
        [Inject] readonly IAudioService _audioService;
        [Inject] readonly IAssetService _assetService;
        [Inject] readonly IPlayerSpawner _playerSpawner;
        [Inject] readonly IProjectileSpawner _projectileSpawner;
        [Inject] readonly IEnemySpawner _enemySpawner;
        [Inject] readonly GameStateModel _gameState;
        [Inject] readonly GameplayModel _gameplay;
        [Inject] readonly EnemyConfig _enemyConfig;
        [Inject] readonly LevelConfig _levelConfig;
        [Inject] readonly AudioConfig _audioConfig;

        private void Start()
        {
            // Load game background
            _background.sharedMaterial = _assetService.Get<Material>(Constants.Materials.Background);
            _background.enabled = true;

            // Handle game state transitions
            _gameState.State.Pairwise()
                .Subscribe(OnStateTransition).AddTo(this);

            // Control enemies during gameplay
            Observable.EveryUpdate().Where(_ => _gameState.State.Value == GameState.Gameplay)
                .Subscribe(_ => GameplayLoop()).AddTo(this);
        }

        private void GameplayLoop()
        {
            HandleEnemyWaves();
            HandleEnemyMovement();
            HandleEnemyShooting();
            DetectEndGame();
        }

        private void HandleEnemyWaves()
        {
            // Spawn the next wave if all enemies are dead
            if (_enemySpawner.Enemies.Count == 0)
            {
                // Spawn enemies at the starting position
                _enemiesManager.Reset();
                _enemySpawner.SpawnAll();

                // Increase wave counter
                _gameplay.CurrentWave.Value++;
            }
        }

        private void HandleEnemyMovement()
        {
            // Move global position of the formation
            var firstEnemy = _enemySpawner.Enemies[0];
            var lastEnemy = _enemySpawner.Enemies[^1];
            _enemiesManager.Move(firstEnemy.transform.position, lastEnemy.transform.position,
                _enemySpawner.Enemies.Count, Time.deltaTime);

            // Update enemy positions
            for (int i = 0; i < _enemySpawner.Enemies.Count; i++)
            {
                EnemyPresenter enemy = _enemySpawner.Enemies[i];
                enemy.transform.position = _enemiesManager.Position + enemy.Position;
            }
        }

        private void HandleEnemyShooting()
        {
            if (_enemiesManager.Shoot(Time.time))
            {
                // Find random enemy horizontal position
                int index = Random.Range(0, _enemySpawner.Enemies.Count);
                float posX = _enemySpawner.Enemies[index].transform.position.x;

                // Find lowest vertical position of the enemies in the specified column
                Vector3 lowestEnemyPos = Vector3.one * float.MaxValue;

                for (int i = 0; i < _enemySpawner.Enemies.Count; i++)
                {
                    Vector3 enemyPos = _enemySpawner.Enemies[i].transform.position;

                    if (Mathf.Approximately(enemyPos.x, posX) && enemyPos.z < lowestEnemyPos.z)
                    {
                        lowestEnemyPos = enemyPos;
                    }
                }

                // Spawn projectile
                Vector3 spawnPos = lowestEnemyPos + Vector3.back * 1.5f;
                _projectileSpawner.Spawn(spawnPos, Vector3.back, _enemyConfig.ProjectileSpeed);

                // Play blaster sfx
                _audioService.PlaySfx(Constants.Audio.Blaster, 0.25f);
            }
        }

        private void DetectEndGame()
        {
            // Find lowest vertical position of the enemies
            Vector3 lowestEnemyPos = Vector3.one * float.MaxValue;

            for (int i = 0; i < _enemySpawner.Enemies.Count; i++)
            {
                Vector3 enemyPos = _enemySpawner.Enemies[i].transform.position;

                if (enemyPos.z < lowestEnemyPos.z)
                {
                    lowestEnemyPos = enemyPos;
                }
            }

            // End game when the enemy with the lowest vertical position gets out of bounds
            if (_levelConfig.IsPosOutOfVerticalBounds(lowestEnemyPos))
            {
                _gameState.State.Value = GameState.Results;
            }
        }

        private void OnStateTransition(Pair<GameState> transition)
        {
            if (transition.Current == GameState.Gameplay)
            {
                OnGameplayStarted();
            }
            else if (transition.Previous == GameState.Gameplay)
            {
                OnGameplayEnded();
            }
        }

        private void OnGameplayStarted()
        {
            _playerSpawner.Spawn();
            _enemiesManager.Reset();
            _audioService.PlayMusic(Constants.Audio.Music, _audioConfig.MusicVolume);
        }

        private void OnGameplayEnded()
        {
            _playerSpawner.Despawn();
            _enemySpawner.DespawnAll();
            _projectileSpawner.DespawnAll();
            _audioService.StopMusic();
        }
    }
}
