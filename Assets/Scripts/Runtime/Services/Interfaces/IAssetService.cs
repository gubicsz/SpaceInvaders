using Cysharp.Threading.Tasks;

namespace SpaceInvaders.Services
{
    /// <summary>
    ///     Manages runtime asset loading.
    /// </summary>
    public interface IAssetService
    {
        /// <summary>
        ///     Loads an asset by key.
        /// </summary>
        /// <typeparam name="T">The type of the asset.</typeparam>
        /// <param name="key">The key.</param>
        UniTask Load<T>(string key);

        /// <summary>
        ///     Gets reference to a loaded asset by key.
        /// </summary>
        /// <typeparam name="T">The type of the asset.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The asset reference.</returns>
        T Get<T>(string key);
    }
}
