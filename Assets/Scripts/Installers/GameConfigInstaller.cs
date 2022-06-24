using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
    public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
    {
        public PlayerConfig Player;

        public override void InstallBindings()
        {
            Container.BindInstances(Player);
        }
    }
}