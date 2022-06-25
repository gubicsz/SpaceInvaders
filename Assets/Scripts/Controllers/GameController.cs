using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class GameController : MonoBehaviour
    {
        [Inject] AddressablesService _addressables;
        [Inject] GameStateModel _gameState;
        [Inject] GameplayModel _gameplay;
        [Inject] PlayerSpawner _playerSpawner;
        [Inject] ProjectileSpawner _projectileSpawner;
        [Inject] EnemySpawner _enemySpawner;
        [Inject] EnemyMover _enemyMover;
        [Inject] LevelConfig _levelConfig;

        private void Start()
        {
            // Handle game loading
            _gameState.State.Where(state => state == GameState.Loading).Subscribe(async state =>
            {
                // Load addressable assets
                await _addressables.Load("Enemy");
                await _addressables.Load("Player");
                await _addressables.Load("Projectile");

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
                    // Spawn enemies
                    _enemySpawner.SpawnAll();

                    // Increase wave counter
                    _gameplay.CurrentWave.Value++;
                }

                // Move enemies
                var firstEnemy = _enemySpawner.Enemies.First();
                var lastEnemy = _enemySpawner.Enemies.Last();
                _enemyMover.Move(firstEnemy.transform.position, lastEnemy.transform.position, Time.deltaTime);

                // End game when the lowest enemy gets out of vertical bounds
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

                // Reset enemy mover
                _enemyMover.Reset();
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
