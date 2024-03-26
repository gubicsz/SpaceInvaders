using SpaceInvaders.Helpers;
using SpaceInvaders.Models;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class GameStateReactor : StateReactor<GameState>
    {
        [Inject]
        private readonly GameStateModel _gameState;

        protected override StateModel<GameState> Model => _gameState;
    }
}
