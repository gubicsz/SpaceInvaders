using Zenject;

namespace SpaceInvaders
{
    public class GameStateReactor : StateReactor<GameState>
    {
        [Inject] GameStateModel _gameState;

        protected override StateModel<GameState> Model => _gameState;
    }
}
