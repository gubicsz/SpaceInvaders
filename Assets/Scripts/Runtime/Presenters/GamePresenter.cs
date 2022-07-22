using Cysharp.Threading.Tasks;
using SpaceInvaders.Models;
using SpaceInvaders.Services;
using System.Linq;
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
        [Inject] readonly GameStateModel _gameState;
        [Inject] readonly GameplayModel _gameplay;
        [Inject] readonly PlayerSpawner _playerSpawner;
        [Inject] readonly ProjectileSpawner _projectileSpawner;
        [Inject] readonly EnemySpawner _enemySpawner;
        [Inject] readonly EnemyConfig _enemyConfig;
        [Inject] readonly LevelConfig _levelConfig;
        [Inject] readonly AudioConfig _audioConfig;

        private void Start()
        {
            // Load game background
            _background.sharedMaterial = _assetService.Get<Material>(Constants.Materials.Background);
            _background.enabled = true;

            // Handle game state transitions
            _gameState.State.Pairwise().Subscribe(transition => OnStateTransition(transition)).AddTo(this);

            // Control enemies during gameplay
            Observable.EveryUpdate().Where(_ => _gameState.State.Value == GameState.Gameplay).Subscribe(_ =>
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

                // Handle enemy movement
                var firstEnemy = _enemySpawner.Enemies.First();
                var lastEnemy = _enemySpawner.Enemies.Last();
                _enemiesManager.Move(firstEnemy.transform.position, lastEnemy.transform.position, 
                    _enemySpawner.Enemies.Count, Time.deltaTime);

                foreach (var enemy in _enemySpawner.Enemies)
                {
                    enemy.transform.position = _enemiesManager.Position + enemy.Position;
                }

                // Handle enemy shooting
                if (_enemiesManager.Shoot(Time.time))
                {
                    // Find random enemy horizontal position
                    int index = Random.Range(0, _enemySpawner.Enemies.Count);
                    float posX = _enemySpawner.Enemies[index].transform.position.x;

                    // Find the enemy with the lowest vertical position in that column
                    var enemyToShoot = _enemySpawner.Enemies
                        .Where(e => Mathf.Approximately(e.transform.position.x, posX))
                        .OrderBy(e => e.transform.position.z)
                        .FirstOrDefault();

                    // Spawn projectile
                    Vector3 spawnPos = enemyToShoot.transform.position + Vector3.back * 1.5f;
                    _projectileSpawner.Spawn(spawnPos, Vector3.back, _enemyConfig.ProjectileSpeed);

                    // Play blaster sfx
                    _audioService.PlaySfx(Constants.Audio.Blaster, 0.25f);
                }

                // End game when the enemy with the lowest vertical position gets out of bounds
                var lowestEnemy = _enemySpawner.Enemies.OrderBy(e => e.transform.position.z).FirstOrDefault();

                if (lowestEnemy != null && _levelConfig.IsPosOutOfVerticalBounds(lowestEnemy.transform.position))
                {
                    _gameState.State.Value = GameState.Results;
                }
            }).AddTo(this);
        }

        private void OnStateTransition(Pair<GameState> transition)
        {
            if (transition.Current == GameState.Gameplay)
            {
                // Spawn player
                _playerSpawner.Spawn();

                // Reset enemies manager
                _enemiesManager.Reset();

                // Start music
                _audioService.PlayMusic(Constants.Audio.Music, _audioConfig.MusicVolume);
            }
            else if (transition.Previous == GameState.Gameplay)
            {
                // Despawn player
                _playerSpawner.Despawn();

                // Despawn enemies
                _enemySpawner.DespawnAll();

                // Despawn projectiles
                _projectileSpawner.DespawnAll();

                // Stop music
                _audioService.StopMusic();
            }
        }
    }
}
