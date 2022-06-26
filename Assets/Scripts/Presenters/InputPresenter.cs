using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class InputPresenter : MonoBehaviour
    {
        [SerializeField] ButtonAxis ButtonHorizontal;
        [SerializeField] ButtonTouch ButtonFire;

        [Inject] InputService _input;
        [Inject] GameStateModel _gameState;

        private const string _axisHorizontal = "Horizontal";
        private const string _buttonFire = "Jump";

        private void Start()
        {
#if UNITY_EDITOR
            // Disable touch buttons in the editor
            ButtonHorizontal.gameObject.SetActive(false);
            ButtonFire.gameObject.SetActive(false);
#endif

            // Update input service based on platform
            Observable.EveryUpdate().Where(_ => _gameState.State.Value == GameState.Gameplay).Subscribe(_ =>
            {
#if UNITY_EDITOR
                _input.Horizontal = Input.GetAxisRaw(_axisHorizontal);
                _input.Fire = Input.GetButton(_buttonFire);
#else
                _input.Horizontal = ButtonHorizontal.Axis;
                _input.Fire = ButtonFire.IsPressed;
#endif
            }).AddTo(this);
        }
    }
}
