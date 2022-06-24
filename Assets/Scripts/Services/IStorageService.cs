namespace SpaceInvaders
{
    public interface IStorageService
    {
        /// <summary>
        /// Saves an object to storage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The key of the object.</param>
        /// <param name="obj">The object to save.</param>
        void Save<T>(string key, T obj);

        /// <summary>
        /// Loads an object from sorage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The key of the object.</param>
        /// <returns>The loaded object.</returns>
        T Load<T>(string key);
    }
}
