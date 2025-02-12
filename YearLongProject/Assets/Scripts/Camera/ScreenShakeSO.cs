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
    }
}