using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace Camera
{
    public class CameraFramingTarget : MonoBehaviour
    {
        [field: BoldHeader("Camera Framing Target")]
        [field: InfoBox("Marks this GameObject as a target for the camera to frame in the shot")]
        [field: SerializeField]
        public bool IsTargeted { get; set; } = true;
    }
}