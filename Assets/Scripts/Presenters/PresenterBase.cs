namespace SpaceInvaders
{
    public class PresenterBase<M, V> : DisposableEntity
		where M : class
		where V : class
	{
		protected M Model { get; }
		protected V View { get; }

		public PresenterBase(M model, V view)
		{
			Model = model;
			View = view;
		}
	}
}
