using System;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SpaceInvaders.Models;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Tests
{
    [TestFixture]
    public class PlayerTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<PlayerConfig>().AsSingle();
            Container.Bind<LevelConfig>().AsSingle();
            Container.Bind<PlayerModel>().AsSingle();
            Container.Inject(this);
        }

        [Inject]
        private readonly PlayerModel _player;

        [Inject]
        private readonly PlayerConfig _config;

        [Inject]
        private readonly LevelConfig _level;

        [Test]
        public void PlayerShouldStartAtSpawnPosition()
        {
            Assert.That(_player.Position.Value == _config.SpawnPosition);
        }

        [Test]
        public void PlayerShouldBeAliveAtStart()
        {
            Assert.That(_player.Lives.Value == _config.Lives);
            Assert.That(!_player.IsDead.Value);
        }

        [Test]
        public void PlayerShouldBeVulnerableAtStart()
        {
            Assert.That(!_player.IsInvulnerable.Value);
        }

        [Test]
        public async void PlayerShouldBeDamagableAtStart()
        {
            var isDamaged = await _player.DamageAsync();

            Assert.That(isDamaged);
        }

        [Test]
        public async void PlayerShouldNotBeDamagableWhenDead()
        {
            _player.Lives.Value = 0;

            var isDamaged = await _player.DamageAsync();

            Assert.That(!isDamaged);
        }

        [Test]
        public async void PlayerShouldNotBeDamagableWhenInvulnerable()
        {
            _player.IsInvulnerable.Value = true;

            var isDamaged = await _player.DamageAsync();

            Assert.That(!isDamaged);
        }

        [Test]
        public void PlayerShouldLoseALifeWhenHit()
        {
            var lives = _player.Lives.Value;
            _player.DamageAsync().Forget();

            Assert.That(_player.Lives.Value == lives - 1);
        }

        [Test]
        public void PlayerShouldBecomeInvulnerableWhenHit()
        {
            _player.DamageAsync().Forget();

            Assert.That(_player.IsInvulnerable.Value);
        }

        [Test]
        public async void PlayerShouldBecomeVulnerableAfterBeingDamaged()
        {
            await _player.DamageAsync();
            await UniTask.Delay(100);

            Assert.That(!_player.IsInvulnerable.Value);
        }

        [Test]
        public async void PlayerShouldBeInvulnerableForTheSpecifiedDuration()
        {
            _player.DamageAsync().Forget();

            Assert.That(_player.IsInvulnerable.Value);

            await UniTask.Delay(TimeSpan.FromSeconds(_config.Invulnerability));
            await UniTask.Delay(100);

            Assert.That(!_player.IsInvulnerable.Value);
        }

        [Test]
        public void PlayerShouldBeResetedProperly()
        {
            _player.Position.Value = Vector3.right;
            _player.DamageAsync().Forget();
            _player.Shoot(10f);

            _player.Reset();

            Assert.That(_player.Position.Value == _config.SpawnPosition);
            Assert.That(_player.Lives.Value == _config.Lives);
            Assert.That(!_player.IsInvulnerable.Value);
            Assert.That(_player.ShotTime == float.MinValue);
        }

        [Test]
        public void PlayerShouldMoveByTheExactAmount()
        {
            var h = 1f;
            var dt = 1f / 60f;
            var x = h * dt * _config.Speed;

            _player.Move(h, dt);

            Assert.That(_player.Position.Value.x == x);
        }

        [Test]
        public void PlayerShouldNotMoveOutOfBounds()
        {
            var h = 1f;
            var dt = 1f / 60f;
            _player.Position.Value = new Vector3(
                _level.Bounds.x,
                _player.Position.Value.y,
                _player.Position.Value.z
            );

            _player.Move(h, dt);

            Assert.That(_player.Position.Value.x == _level.Bounds.x);
        }

        [Test]
        public void PlayerShouldBeAbleToShootAtStart()
        {
            var currentTime = 0f;

            var shot = _player.Shoot(currentTime);

            Assert.That(shot);
            Assert.That(_player.ShotTime == currentTime);
        }

        [Test]
        public void PlayerShouldNotBeAbleToShootTwiceAtTheSameTime()
        {
            var shot1 = _player.Shoot(0f);
            var shot2 = _player.Shoot(0f);

            Assert.That(shot1);
            Assert.That(!shot2);
        }

        [Test]
        public void PlayerShouldBeAbleToShootBasedOnFireRate()
        {
            var shot1 = _player.Shoot(0f);
            var shot2 = _player.Shoot(_config.FireRate);

            Assert.That(shot1);
            Assert.That(shot2);
        }
    }
}
