using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Basic/VoidEvent")]
    public class VoidEvent : SoEvent
    {
        public void Raise()
        {
#if UNITY_EDITOR
            Debug.Log($"Void Event {name} raised");
#endif
            InternalVoidEvent?.Invoke();
        }
    }
}