using Events;
using UnityEngine;

namespace Camera
{
    [CreateAssetMenu(fileName = "CameraTargetEvent", menuName = "Events/CameraTargetEvent")]
    public class CameraTargetEvent : SoEvent<CameraFramingTarget>
    {
    }
}