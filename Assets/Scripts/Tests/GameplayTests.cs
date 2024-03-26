using NUnit.Framework;
using SpaceInvaders.Models;
using Zenject;

namespace SpaceInvaders.Tests
{
    [TestFixture]
    public class GameplayTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<GameplayModel>().AsSingle();
            Container.Inject(this);
        }

        [Inject]
        private readonly GameplayModel _gameplay;

        [Test]
        public void GameplayShouldStartWithInitialValues()
        {
            Assert.That(_gameplay.CurrentScore.Value == 0);
            Assert.That(_gameplay.CurrentWave.Value == 0);
        }

        [Test]
        public void GameplayResetShouldZeroValues()
        {
            _gameplay.CurrentScore.Value = 1000;
            _gameplay.CurrentWave.Value = 2;

            _gameplay.Reset();

            Assert.That(_gameplay.CurrentScore.Value == 0);
            Assert.That(_gameplay.CurrentWave.Value == 0);
        }
    }
}
