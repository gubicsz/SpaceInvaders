using UnityEngine;

namespace SpaceInvaders.Services
{
    /// <summary>
    ///     Manages enemy grid formation movement and shooting.
    /// </summary>
    public interface IEnemiesManager
    {
        /// <summary>
        ///     The global position of the enemy grid formation.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        ///     The direction of the formation movement (left or right).
        /// </summary>
        Vector3 Direction { get; }

        /// <summary>
        ///     Resets the manager.
        /// </summary>
        void Reset();

        /// <summary>
        ///     Updates the position and direction based on the formation's left and right positions.
        /// </summary>
        /// <param name="leftPos">The left position of the formation.</param>
        /// <param name="rightPos">The right position of the formation.</param>
        /// <param name="enemyCount">The number of enemies in the formation.</param>
        /// <param name="dt">The delta time.</param>
        void Move(Vector3 leftPos, Vector3 rightPos, int enemyCount, float dt);

        /// <summary>
        ///     Handles firing behavour of the formation based on the current time.
        /// </summary>
        /// <param name="time">The current time.</param>
        /// <returns>True if the shot occured, false otherwise.</returns>
        bool Shoot(float time);
    }
}
