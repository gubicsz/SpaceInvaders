using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SpaceInvaders
{
    public abstract class StateReactor<T> : MonoBehaviour where T : Enum
    {
        [SerializeField]
        private List<T> _visibleInStates;

        protected abstract StateModel<T> Model { get; }

        private void Start()
        {
            Model.State.Subscribe(state => 
                gameObject.SetActive(IsVisible(state))).AddTo(this);
        }

        private bool IsVisible(T current)
        {
            if (current == null)
            {
                return false;
            }

            return _visibleInStates.Contains(current);
        }
    }
}
