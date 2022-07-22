using SpaceInvaders.Helpers;
using SpaceInvaders.Models;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class InputPresenter : MonoBehaviour
    {
        [SerializeField] ButtonAxis ButtonHorizontal;
        [SerializeField] ButtonTouch ButtonFire;

        [Inject] readonly InputModel _input;
        [Inject] readonly GameStateModel _gameState;

        private const string _axisHorizontal = "Horizontal";
        private const string _buttonFire = "Jump";

        private void Start()
        {
#if UNITY_EDITOR
            // Disable touch buttons in the editor
            ButtonHorizontal.gameObject.SetActive(false);
            ButtonFire.gameObject.SetActive(false);
#endif

            // Update input model based on platform
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
