using NUnit.Framework;
using SpaceInvaders;
using Zenject;

[TestFixture]
public class PlayerTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        Container.Bind<PlayerConfig>().AsSingle();
        Container.Bind<LevelConfig>().AsSingle();
        Container.Bind<PlayerModel>().AsSingle();
    }

    [Test]
    public void PlayerShouldStartAtSpawnPosition()
    {
        var player = Container.Resolve<PlayerModel>();
        var config = Container.Resolve<PlayerConfig>();

        Assert.That(player.Position.Value == config.SpawnPosition);
    }

    [Test]
    public void PlayerShouldBeAliveAtStart()
    {
        var player = Container.Resolve<PlayerModel>();
        var config = Container.Resolve<PlayerConfig>();

        Assert.That(player.Lives.Value == config.Lives);
        Assert.That(!player.IsDead.Value);
    }

    [Test]
    public void PlayerShouldBeVulnerableAtStart()
    {
        var player = Container.Resolve<PlayerModel>();

        Assert.That(!player.IsInvulnerable.Value);
    }

    [Test]
    public void PlayerShouldBeDamagableAtStart()
    {
        var player = Container.Resolve<PlayerModel>();

        Assert.That(player.DamageCommand.CanExecute.Value);
    }

    [Test]
    public void PlayerShouldNotBeDamagableWhenDead()
    {
        var player = Container.Resolve<PlayerModel>();
        player.Lives.Value = 0;

        Assert.That(!player.DamageCommand.CanExecute.Value);
    }

    [Test]
    public void PlayerShouldNotBeDamagableWhenInvulnerable()
    {
        var player = Container.Resolve<PlayerModel>();
        player.IsInvulnerable.Value = true;

        Assert.That(!player.DamageCommand.CanExecute.Value);
    }

    [Test]
    public void PlayerShouldLoseALifeWhenHit()
    {
        var player = Container.Resolve<PlayerModel>();
        int lives = player.Lives.Value;
        player.DamageCommand.Execute();

        Assert.That(player.Lives.Value == (lives - 1));
    }

    [Test]
    public void PlayerShouldBecomeInvulnerableWhenHit()
    {
        var player = Container.Resolve<PlayerModel>();
        player.DamageCommand.Execute();

        Assert.That(player.IsInvulnerable.Value);
    }
}