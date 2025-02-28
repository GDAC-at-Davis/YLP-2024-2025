using Base;
using UnityEngine;

namespace Camera
{
    public class CameraFramingTarget : DescriptionMono
    {
        [field: SerializeField]
        public bool IsTargeted { get; set; } = true;
    }
}