using UnityEngine;

namespace SpaceInvaders.Helpers
{
    public class ButtonAxis : MonoBehaviour
    {
        [SerializeField]
        private ButtonTouch _buttonNegative;

        [SerializeField]
        private ButtonTouch _buttonPositive;
        public float Axis { get; private set; }

        private void Update()
        {
            // Update axis based on touch buttons
            Axis = _buttonNegative.IsPressed
                ? -1f
                : _buttonPositive.IsPressed
                    ? 1f
                    : 0f;
        }
    }
}
