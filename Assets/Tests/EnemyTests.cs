using NSubstitute;
using NUnit.Framework;
using SpaceInvaders;
using UniRx;
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
        Container.Bind<IEnemiesManager>().FromSubstitute();
        Container.Inject(this);
    }

    [Inject] EnemyModel _enemy;
    [Inject] EnemyConfig _enemyConfig;
    [Inject] IEnemiesManager _enemiesManager;

    [Test]
    public void InitShouldSetProperties()
    {
        int row = 0;
        int col = 0;
        _enemiesManager.Position.Returns(new Vector3ReactiveProperty(Vector3.zero));

        _enemy.Init(row, col);

        Assert.That(_enemy.Row == row);
        Assert.That(_enemy.Col == col);
        Assert.That(_enemy.Position != null);
    }

    [Test]
    public void InitShouldSetCorrectPosition()
    {
        _enemiesManager.Position.Returns(new Vector3ReactiveProperty(Vector3.zero));

        Vector3 centerPos = -_enemy.CalculateGridPosition((_enemyConfig.Columns - 1) / 2, 0);

        if (_enemyConfig.Columns % 2 == 0)
        {
            centerPos -= _enemyConfig.GridWidth / 2f * Vector3.right;
        }

        _enemy.Init(0, 0);

        Assert.That(_enemy.Position.Value == centerPos);
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
