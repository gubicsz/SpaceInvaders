using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceInvaders.Helpers
{
    public class ButtonTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }

        private void OnDisable()
        {
            IsPressed = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }
    }
}
