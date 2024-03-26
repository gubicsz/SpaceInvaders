using System;
using UniRx;

namespace SpaceInvaders.Helpers
{
    public abstract class StateModel<T>
        where T : Enum
    {
        public StateModel(T state)
        {
            State = new ReactiveProperty<T>(state);
        }

        public ReactiveProperty<T> State { get; private set; }
    }
}
