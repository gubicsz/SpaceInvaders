namespace SpaceInvaders
{
    public enum GameState
    {
        Loading,
        Menu,
        Gameplay,
        Results,
        Scores,
    }

    public class GameStateModel : StateModel<GameState>
    {
        public GameStateModel() : base(GameState.Loading)
        {

        }
    }
}
