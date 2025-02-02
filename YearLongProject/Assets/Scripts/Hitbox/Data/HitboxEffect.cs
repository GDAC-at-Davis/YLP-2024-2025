using System;
using UnityEngine;

namespace Hitbox.DataStructures
{
    /// <summary>
    ///     Data structure with information about what happens when a hitbox hits a target
    /// </summary>
    [Serializable]
    public class HitboxEffect
    {
        [field: SerializeField]
        public float Damage { get; set; }

        [field: SerializeField]
        public Vector2 Knockback { get; set; }
    }
}