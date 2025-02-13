using Base;
using UnityEngine;

namespace Camera
{
    /// <summary>
    ///     Scriptable object container for screen shake effect
    /// </summary>
    [CreateAssetMenu(fileName = "ScreenShake", menuName = "Effects/ScreenShake")]
    public class ScreenShakeEffectSO : DescriptionSO
    {
        [field: SerializeField]
        public ScreenShakeEffect ScreenShakeEffect { get; set; }
    }
}