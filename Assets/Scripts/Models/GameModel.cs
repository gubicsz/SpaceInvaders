using UniRx;

namespace SpaceInvaders
{
    public class GameModel : DisposableEntity
    {
        private AssetLoader _loader;
        private GameStateModel _gameState;

        public GameModel(AssetLoader loader, GameStateModel gameState)
        {
            // Set references
            _loader = loader;
            _gameState = gameState;

            // Handle state change
            _gameState.State.Subscribe(state => OnStateChanged(state)).AddTo(this);
        }

        private async void OnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Loading:
                    // Load addressable assets
                    await _loader.LoadAsset("Enemy");
                    await _loader.LoadAsset("Player");
                    await _loader.LoadAsset("Projectile");

                    // Proceed to the main menu
                    _gameState.State.Value = GameState.Menu;
                    break;

                case GameState.Menu:
                    break;

                case GameState.Gameplay:
                    // todo: reset game
                    break;

                case GameState.Results:
                    break;

                case GameState.Scores:
                    break;

                default:
                    break;
            }
        }
    }
}
