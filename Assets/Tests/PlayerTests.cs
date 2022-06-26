using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SpaceInvaders;
using UnityEngine;
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
    public async void PlayerShouldBeDamagableAtStart()
    {
        var player = Container.Resolve<PlayerModel>();

        bool isDamaged = await player.DamageAsync();

        Assert.That(isDamaged);
    }

    [Test]
    public async void PlayerShouldNotBeDamagableWhenDead()
    {
        var player = Container.Resolve<PlayerModel>();
        player.Lives.Value = 0;

        bool isDamaged = await player.DamageAsync();

        Assert.That(!isDamaged);
    }

    [Test]
    public async void PlayerShouldNotBeDamagableWhenInvulnerable()
    {
        var player = Container.Resolve<PlayerModel>();
        player.IsInvulnerable.Value = true;

        bool isDamaged = await player.DamageAsync();

        Assert.That(!isDamaged);
    }

    [Test]
    public void PlayerShouldLoseALifeWhenHit()
    {
        var player = Container.Resolve<PlayerModel>();
        int lives = player.Lives.Value;
        player.DamageAsync().Forget();

        Assert.That(player.Lives.Value == (lives - 1));
    }

    [Test]
    public void PlayerShouldBecomeInvulnerableWhenHit()
    {
        var player = Container.Resolve<PlayerModel>();
        player.DamageAsync().Forget();

        Assert.That(player.IsInvulnerable.Value);
    }

    [Test]
    public async void PlayerShouldBecomeVulnerableAfterBeingDamaged()
    {
        var player = Container.Resolve<PlayerModel>();
        await player.DamageAsync();

        Assert.That(!player.IsInvulnerable.Value);
    }

    [Test]
    public async void PlayerShouldBeInvulnerableForTheSpecifiedDuration()
    {
        var player = Container.Resolve<PlayerModel>();
        var config = Container.Resolve<PlayerConfig>();
        player.DamageAsync().Forget();

        Assert.That(player.IsInvulnerable.Value);

        await UniTask.Delay(System.TimeSpan.FromSeconds(config.Invulnerability));

        Assert.That(!player.IsInvulnerable.Value);
    }

    [Test]
    public void PlayerShouldBeResetedProperly()
    {
        var player = Container.Resolve<PlayerModel>();
        var config = Container.Resolve<PlayerConfig>();
        player.Position.Value = Vector3.right;
        player.DamageAsync().Forget();
        player.Shoot(10f);

        player.Reset();

        Assert.That(player.Position.Value == config.SpawnPosition);
        Assert.That(player.Lives.Value == config.Lives);
        Assert.That(!player.IsInvulnerable.Value);
        Assert.That(player.ShotTime == float.MinValue);
    }

    [Test]
    public void PlayerShouldMoveByTheExactAmount()
    {
        var player = Container.Resolve<PlayerModel>();
        var config = Container.Resolve<PlayerConfig>();
        float h = 1f;
        float dt = 1f / 60f;
        float x = h * dt * config.Speed;

        player.Move(h, dt);

        Assert.That(player.Position.Value.x == x);
    }

    [Test]
    public void PlayerShouldNotMoveOutOfBounds()
    {
        var player = Container.Resolve<PlayerModel>();
        var level = Container.Resolve<LevelConfig>();

        float h = 1f;
        float dt = 1f / 60f;
        player.Position.Value = new Vector3(level.Bounds.x, 
            player.Position.Value.y, player.Position.Value.z);

        player.Move(h, dt);

        Assert.That(player.Position.Value.x == level.Bounds.x);
    }

    [Test]
    public void PlayerShouldBeAbleToShootAtStart()
    {
        var player = Container.Resolve<PlayerModel>();
        float currentTime = 0f;

        bool shot = player.Shoot(currentTime);

        Assert.That(shot);
        Assert.That(player.ShotTime == currentTime);
    }

    [Test]
    public void PlayerShouldNotBeAbleToShootTwiceAtTheSameTime()
    {
        var player = Container.Resolve<PlayerModel>();

        bool shot1 = player.Shoot(0f);
        bool shot2 = player.Shoot(0f);

        Assert.That(shot1);
        Assert.That(!shot2);
    }

    [Test]
    public void PlayerShouldBeAbleToShootBasedOnFireRate()
    {
        var player = Container.Resolve<PlayerModel>();
        var config = Container.Resolve<PlayerConfig>();

        bool shot1 = player.Shoot(0f);
        bool shot2 = player.Shoot(config.FireRate);

        Assert.That(shot1);
        Assert.That(shot2);
    }
}