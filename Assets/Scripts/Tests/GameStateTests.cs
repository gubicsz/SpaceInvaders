using NUnit.Framework;
using SpaceInvaders;
using Zenject;

[TestFixture]
public class GameStateTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        Container.Bind<GameStateModel>().AsSingle();
        Container.Inject(this);
    }

    [Inject] GameStateModel _gameState;

    [Test]
    public void GameStateShouldBeLoadingAtStart()
    {
        Assert.That(_gameState.State.Value == GameState.Loading);
    }
}
