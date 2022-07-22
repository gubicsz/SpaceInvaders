using NUnit.Framework;
using SpaceInvaders;
using UnityEngine;
using Zenject;

[TestFixture]
public class ProjectileTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        Container.Bind<ProjectileModel>().AsSingle();
        Container.Bind<LevelConfig>().AsSingle();
        Container.Inject(this);
    }

    [Inject] ProjectileModel _projectile;
    [Inject] LevelConfig _level;

    [Test]
    public void ProjectileShouldStartWithInitialValues()
    {
        Assert.That(_projectile.Position == Vector3.zero);
        Assert.That(_projectile.Direction == Vector3.zero);
        Assert.That(_projectile.Speed == 0f);
    }

    [Test]
    public void InitShouldSetProperties()
    {
        Vector3 position = Vector3.one;
        Vector3 direction = Vector3.forward;
        float speed = 10f;

        _projectile.Init(position, direction, speed);

        Assert.That(_projectile.Position == position);
        Assert.That(_projectile.Direction == direction);
        Assert.That(_projectile.Speed == speed);
    }

    [Test]
    public void ResetShouldZeroProperties()
    {
        _projectile.Init(Vector3.one, Vector3.forward, 10f);
        _projectile.Reset();

        Assert.That(_projectile.Position == Vector3.zero);
        Assert.That(_projectile.Direction == Vector3.zero);
        Assert.That(_projectile.Speed == 0f);
    }

    [Test]
    public void MoveShouldChangePositionBasedOnSpeedDirectionAndTime()
    {
        _projectile.Init(Vector3.zero, Vector3.forward, 1f);

        bool isOutOfBounds = _projectile.Move(1f);

        Assert.That(_projectile.Position == Vector3.forward);
        Assert.That(!isOutOfBounds);
    }

    [Test]
    public void MoveShouldIndicateOutOfLevelBounds()
    {
        _projectile.Init(_level.Bounds, Vector3.forward, 1f);

        bool isOutOfBounds = _projectile.Move(1f);

        Assert.That(isOutOfBounds);
    }
}
