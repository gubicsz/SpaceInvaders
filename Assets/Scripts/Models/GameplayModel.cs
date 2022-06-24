using UniRx;

namespace SpaceInvaders
{
    public class GameplayModel
    {
        public ReactiveProperty<int> CurrentScore { get; private set; }

        public ReactiveProperty<int> CurrentWave { get; private set; }

        public GameplayModel()
        {
            CurrentScore = new ReactiveProperty<int>(0);
            CurrentWave = new ReactiveProperty<int>(0);
        }

        public void Reset()
        {
            CurrentScore.Value = 0;
            CurrentWave.Value = 0;
        }
    }
}
