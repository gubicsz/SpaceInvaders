using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class PlayerConfig
    {
        public int Lives = 3;
        public float Invulnerability = 3.0f;
        //todo: speed
    }

    [CreateAssetMenu(fileName = "GameConfig", menuName = "SpaceInvaders/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField]
        public PlayerConfig player;
    }
}
