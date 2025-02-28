using UnityEngine;

namespace Utils
{
    public class SetFPS : MonoBehaviour
    {
        [SerializeField]
        private int targetFrameRate;

        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}