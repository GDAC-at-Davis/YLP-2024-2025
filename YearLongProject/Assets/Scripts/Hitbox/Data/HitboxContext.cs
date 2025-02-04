using UnityEngine;
using Hitbox.Emitters;

namespace Hitbox.DataStructures
{
    /// <summary>
    ///     Context for a hitbox, e.g the position it's being created from
    /// </summary>
    public class HitboxContext
    {
        /// <summary>
        ///     Layer mask to check for
        /// </summary>
        public LayerMask LayerMask;

        /// <summary>
        ///     Position of the "origin" for the hitbox area.
        ///     Generally related to the entity performing the action this hitbox is for.
        /// </summary>
        public Vector2 SourcePosition;

        /// <summary>
        ///     Rotation of the "origin" for the hitbox area.
        /// </summary>
        public float SourceAngle;

        /// <summary>
        ///     Whether character is flipped or not. False means pointing right (+x direction)
        /// </summary>
        public bool FlipX;

        /// <summary>
        ///     Character that created the hitbox
        /// </summary>
        public HitboxEmitter Source;

        /// <summary>
        ///     ID of the attack that hitbox belongs to
        /// </summary>
        public string HitboxID;
    }
}