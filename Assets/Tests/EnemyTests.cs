using NUnit.Framework;
using SpaceInvaders;
using UnityEngine;
using Zenject;

[TestFixture]
public class EnemyTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        Container.Bind<EnemyModel>().AsSingle();
        Container.Bind<EnemyConfig>().AsSingle();
        Container.Inject(this);
    }

    [Inject] EnemyModel _enemy;
    [Inject] EnemyConfig _enemyConfig;

    [Test]
    public void InitShouldSetProperties()
    {
        int type = 0;
        int row = 0;
        int col = 0;

        _enemy.Init(type, row, col);

        Assert.That(_enemy.Type == type);
        Assert.That(_enemy.Row == row);
        Assert.That(_enemy.Col == col);
        Assert.That(_enemy.Position != Vector3.zero);
    }

    [Test]
    public void InitShouldSetCorrectPosition()
    {
        Vector3 centerPos = -_enemy.CalculateGridPosition((_enemyConfig.Columns - 1) / 2, 0);

        if (_enemyConfig.Columns % 2 == 0)
        {
            centerPos -= _enemyConfig.GridWidth / 2f * Vector3.right;
        }

        _enemy.Init(0, 0, 0);

        Assert.That(_enemy.Position == centerPos);
    }

    [Test]
    public void ResetShouldZeroProperties()
    {
        _enemy.Init(0, 0, 0);

        _enemy.Reset();

        Assert.That(_enemy.Type == 0);
        Assert.That(_enemy.Row == 0);
        Assert.That(_enemy.Col == 0);
        Assert.That(_enemy.Position == Vector3.zero);
    }

    [Test]
    public void VerifyCalculateGridPosition()
    {
        int col = 1;
        int row = 1;
        Vector3 gridPosition =
            (_enemyConfig.GridWidth * col * Vector3.right) +
            (_enemyConfig.GridHeight * row * Vector3.forward);

        Vector3 calculatedPosition = _enemy.CalculateGridPosition(col, row);

        Assert.That(calculatedPosition == gridPosition);
    }
}
