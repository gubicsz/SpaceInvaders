using UnityEngine;

namespace SpaceInvaders
{
    public interface IAudioService
    {
        /// <summary>
        /// Plays a sound effect at the specified position.
        /// </summary>
        /// <param name="key">The key of the audio clip.</param>
        /// <param name="position">The position to play at.</param>
        /// <param name="volume">The volume.</param>
        void PlaySfx(string key, float volume);
    }
}
