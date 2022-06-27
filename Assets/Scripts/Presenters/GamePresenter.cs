using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class GamePresenter : MonoBehaviour
    {
        [Inject] IAssetService _assetService;
        [Inject] IEnemiesManager _enemiesManager;
        [Inject] GameStateModel _gameState;
        [Inject] GameplayModel _gameplay;
        [Inject] ScoresModel _scores;
        [Inject] PlayerSpawner _playerSpawner;
        [Inject] ProjectileSpawner _projectileSpawner;
        [Inject] EnemySpawner _enemySpawner;
        [Inject] EnemyConfig _enemyConfig;
        [Inject] LevelConfig _levelConfig;

        private void Start()
        {
            // Handle game loading
            _gameState.State.Where(state => state == GameState.Loading).Subscribe(async state =>
            {
                // Load addressable assets
                await _assetService.Load<GameObject>("Enemy1");
                await _assetService.Load<GameObject>("Enemy2");
                await _assetService.Load<GameObject>("Enemy3");
                await _assetService.Load<GameObject>("Player");
                await _assetService.Load<GameObject>("Projectile");

                // Load scores
                _scores.Load();

                // Proceed to the main menu
                _gameState.State.Value = GameState.Menu;
            }).AddTo(this);

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
                _enemiesManager.Move(firstEnemy.transform.position, lastEnemy.transform.position, Time.deltaTime);

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
            }
            else if (transition.Previous == GameState.Gameplay)
            {
                // Despawn player
                _playerSpawner.Despawn();

                // Despawn enemies
                _enemySpawner.DespawnAll();

                // Despawn projectiles
                _projectileSpawner.DespawnAll();
            }
        }
    }
}
