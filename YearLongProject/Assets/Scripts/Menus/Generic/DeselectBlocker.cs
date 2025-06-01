using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menus
{
    /// <summary>
    ///     Prevents deselecting UI elements when the player clicks outside of the UI.
    /// </summary>
    public class DeselectBlocker : MonoBehaviour
    {
        [ShowNonSerializedField]
        private GameObject lastSelectedElement;

        private void Update()
        {
            GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastSelectedElement);
            }
            else
            {
                lastSelectedElement = selectedGameObject;
            }
        }
    }
}