using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class PlayerController : MonoBehaviour
    {
        [Inject]
        PlayerModel _player;

        private void Start()
        {
            
        }
    }
}
