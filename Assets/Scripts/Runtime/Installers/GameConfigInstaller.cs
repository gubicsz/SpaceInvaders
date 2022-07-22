using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
    public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
    {
        [SerializeField] PlayerConfig _player;
        [SerializeField] EnemyConfig _enemy;
        [SerializeField] LevelConfig _level;
        [SerializeField] AudioConfig _audio;

        public override void InstallBindings()
        {
            // Configs
            Container.BindInstances(_player);
            Container.BindInstances(_enemy);
            Container.BindInstances(_level);
            Container.BindInstances(_audio);
        }
    }
}