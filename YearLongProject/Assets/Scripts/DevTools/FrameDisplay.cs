using UnityEngine;

namespace DevTools
{
    public class FrameDisplay : MonoBehaviour
    {
        private int fixedFrame;

        private void FixedUpdate()
        {
            fixedFrame++;
        }

        private void OnGUI()
        {
#if UNITY_EDITOR
            GUI.Label(new Rect(10, 10, 200, 20), $"{fixedFrame}");
#endif
        }
    }
}