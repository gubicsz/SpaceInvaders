using NUnit.Framework;
using SpaceInvaders.Models;
using Zenject;

namespace SpaceInvaders.Tests
{
    [TestFixture]
    public class GameStateTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<GameStateModel>().AsSingle();
            Container.Inject(this);
        }

        [Inject]
        private readonly GameStateModel _gameState;

        [Test]
        public void GameStateShouldBeLoadingAtStart()
        {
            Assert.That(_gameState.State.Value == GameState.Loading);
        }
    }
}
