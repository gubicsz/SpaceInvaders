namespace SpaceInvaders.Services
{
    public interface IAudioService
    {
        /// <summary>
        ///     Plays a sound effect at the specified position.
        /// </summary>
        /// <param name="key">The key of the audio clip.</param>
        /// <param name="position">The position to play at.</param>
        /// <param name="volume">The volume.</param>
        void PlaySfx(string key, float volume);

        /// <summary>
        ///     Starts music playback.
        /// </summary>
        /// <param name="key">The key of the audio clip.</param>
        /// <param name="volume">The volume.</param>
        void PlayMusic(string key, float volume);

        /// <summary>
        ///     Stops music playback.
        /// </summary>
        void StopMusic();

        /// <summary>
        ///     Lowers the volume of the music then automatically increases it over the duration.
        /// </summary>
        /// <param name="targetVolume">The min volume.</param>
        /// <param name="originalVolume">The max volume.</param>
        /// <param name="duration">The duration of the effect.</param>
        void DuckMusic(float targetVolume, float originalVolume, float duration);
    }
}
