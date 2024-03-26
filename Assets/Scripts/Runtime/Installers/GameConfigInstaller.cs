using SpaceInvaders.Models;
using SpaceInvaders.Services;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Installers
{
    [CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
    public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
    {
        [SerializeField]
        private PlayerConfig _player;

        [SerializeField]
        private EnemyConfig _enemy;

        [SerializeField]
        private LevelConfig _level;

        [SerializeField]
        private AudioConfig _audio;

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
