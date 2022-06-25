using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class InputController : MonoBehaviour
    {
        [Inject] InputService _input;
        [Inject] GameStateModel _gameState;

        private const string _axisHorizontal = "Horizontal";
        private const string _buttonFire = "Jump";

        private void Start()
        {
            // Update input service based on platform
            Observable.EveryUpdate().Where(_ => _gameState.State.Value == GameState.Gameplay).Subscribe(_ =>
            {
#if UNITY_EDITOR
                _input.Horizontal = Input.GetAxisRaw(_axisHorizontal);
                _input.Fire = Input.GetButton(_buttonFire);
#else
                // todo: touch buttons (left / right / fire)
#endif
            }).AddTo(this);
        }
    }
}
