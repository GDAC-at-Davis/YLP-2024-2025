using Base;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    /// <summary>
    ///     Defines a screen shake effect
    /// </summary>
    [CreateAssetMenu(fileName = "ScreenShake", menuName = "Effects/ScreenShake")]
    public class ScreenShakeSO : DescriptionSO
    {
        [field: SerializeField]
        public CinemachineImpulseDefinition ImpulseDefinition { get; set; }

        [field: SerializeField]
        public Vector2 Velocity { get; set; }

        [field: Tooltip("Flip the X velocity when emitting the impulse based on the characters's facing direction")]
        [field: SerializeField]
        public bool FlipXVelocity { get; set; }
    }
}