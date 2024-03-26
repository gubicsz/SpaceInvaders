using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Helpers
{
    /// <summary>
    ///     Controls the visibility of an object based on the specified states.
    /// </summary>
    public abstract class StateReactor<T> : MonoBehaviour
        where T : Enum
    {
        [Tooltip("List of states in which this object should be visible.")]
        [SerializeField]
        private List<T> _visibleInStates;

        protected abstract StateModel<T> Model { get; }

        private void Start()
        {
            // Update visibility based on state
            Model.State.Subscribe(state => gameObject.SetActive(IsVisible(state))).AddTo(this);
        }

        private bool IsVisible(T state)
        {
            // Handle error
            if (state == null)
                return false;

            // Indicate whether the state is visible or not
            return _visibleInStates.Contains(state);
        }
    }
}
