using NSubstitute;
using NUnit.Framework;
using SpaceInvaders;
using System;
using System.Linq;
using Zenject;

[TestFixture]
public class ScoresTests : ZenjectUnitTestFixture
{
    [SetUp]
    public void CommonInstall()
    {
        Container.Bind<ScoresModel>().AsSingle();
        Container.Bind<IStorageService>().FromSubstitute();
        Container.Inject(this);
    }

    [Inject] ScoresModel _scores;
    [Inject] IStorageService _storageService;

    Scoreboard _testScoreboard = new Scoreboard()
    {
        Items = new ScoreItem[]
        {
            new ScoreItem() { Score = 1230, Date = DateTime.Now.AddDays(-2).ToString("MM/dd/yyyy HH:mm") },
            new ScoreItem() { Score = 150, Date = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy HH:mm") },
            new ScoreItem() { Score = 90, Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm") },
        }
    };

    [Test]
    public void ScoreboardShouldNotBeNullAtStart()
    {
        Assert.That(_scores.Scoreboard != null);
    }

    [Test]
    public void SaveShouldCallStorageService()
    {
        _scores.Save();

        _storageService.Received().Save(Arg.Any<string>(), Arg.Any<Scoreboard>());
    }

    [Test]
    public void LoadShouldCallStorageService()
    {
        _scores.Load();

        _storageService.Received().Load<Scoreboard>(Arg.Any<string>());
    }

    [Test]
    public void LoadShouldNotUpdateScoreboardAtStart()
    {
        int count = _scores.Scoreboard.Count;
        _storageService.Load<Scoreboard>(Arg.Any<string>()).Returns(default(Scoreboard));

        _scores.Load();

        Assert.That(_scores.Scoreboard.Count == count);
    }

    [Test]
    public void LoadShouldUpdateScoreboard()
    {
        _storageService.Load<Scoreboard>(Arg.Any<string>()).Returns(_testScoreboard);

        _scores.Load();

        for (int i = 0; i < _scores.Scoreboard.Count; i++)
        {
            Assert.That(_scores.Scoreboard[i] == _testScoreboard.Items[i]);
        }
    }

    [Test]
    public void AddShouldOrderItemsByScore()
    {
        foreach (var item in _testScoreboard.Items.Reverse())
        {
            _scores.Add(item);
        }

        Assert.That(Enumerable.SequenceEqual(_scores.Scoreboard, _testScoreboard.Items));
    }
}
