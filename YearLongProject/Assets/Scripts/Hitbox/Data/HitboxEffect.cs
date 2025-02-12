using System;
using Camera;
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

        [field: Header("Hit Stop")]

        [field: SerializeField]
        public float HitPauseDuration { get; set; }

        [field: SerializeField]
        public bool GiveTargetHitStop { get; set; }

        [field: SerializeField]
        public bool GiveAttackerHitStop { get; set; }

        [field: Header("Screen Shake")]

        [field: SerializeField]

        public ScreenShakeSO ScreenShakeEffect { get; set; }
    }
}