using UniRx;

namespace SpaceInvaders.Models
{
    public class GameplayModel
    {
        public GameplayModel()
        {
            CurrentScore = new ReactiveProperty<int>(0);
            CurrentWave = new ReactiveProperty<int>(0);
        }

        public ReactiveProperty<int> CurrentScore { get; }

        public ReactiveProperty<int> CurrentWave { get; }

        public void Reset()
        {
            CurrentScore.Value = 0;
            CurrentWave.Value = 0;
        }
    }
}
