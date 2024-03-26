using SpaceInvaders.Helpers;
using SpaceInvaders.Models;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Presenters
{
    public class InputPresenter : MonoBehaviour
    {
        private const string _axisHorizontal = "Horizontal";
        private const string _buttonFire = "Jump";

        [SerializeField]
        private ButtonAxis ButtonHorizontal;

        [SerializeField]
        private ButtonTouch ButtonFire;

        [Inject]
        private readonly GameStateModel _gameState;

        [Inject]
        private readonly InputModel _input;

        private void Start()
        {
#if UNITY_EDITOR
            // Disable touch buttons in the editor
            ButtonHorizontal.gameObject.SetActive(false);
            ButtonFire.gameObject.SetActive(false);
#endif

            // Update input model based on platform
            Observable
                .EveryUpdate()
                .Where(_ => _gameState.State.Value == GameState.Gameplay)
                .Subscribe(_ =>
                {
#if UNITY_EDITOR
                    _input.Horizontal = Input.GetAxisRaw(_axisHorizontal);
                    _input.Fire = Input.GetButton(_buttonFire);
#else
                    _input.Horizontal = ButtonHorizontal.Axis;
                    _input.Fire = ButtonFire.IsPressed;
#endif
                })
                .AddTo(this);
        }
    }
}
