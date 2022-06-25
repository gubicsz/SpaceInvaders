using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
    public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
    {
        [SerializeField] PlayerConfig _player;
        [SerializeField] EnemyConfig _enemy;
        [SerializeField] ProjectileConfig _projectile;
        [SerializeField] LevelConfig _level;

        public override void InstallBindings()
        {
            Container.BindInstances(_player);
            Container.BindInstances(_enemy);
            Container.BindInstances(_projectile);
            Container.BindInstances(_level);
        }
    }
}