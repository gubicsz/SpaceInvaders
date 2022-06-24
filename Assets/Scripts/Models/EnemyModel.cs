using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceInvaders
{
    public class EnemyModel
    {
        // todo: type, position, isdead etc.

        public class Factory : PlaceholderFactory<Object, EnemyModel>
        {
        }
    }
}
