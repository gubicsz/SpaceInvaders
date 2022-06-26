using UnityEngine;

namespace SpaceInvaders
{
    public class ButtonAxis : MonoBehaviour
    {
        public float Axis { get; private set; }

        [SerializeField] ButtonTouch _buttonNegative;
        [SerializeField] ButtonTouch _buttonPositive;

        private void Update()
        {
            // Update axis based on touch buttons
            Axis = _buttonNegative.IsPressed ? -1f : _buttonPositive.IsPressed ? 1f : 0f;
        }
    }
}
