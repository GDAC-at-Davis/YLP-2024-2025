using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Utils
{
    public class SceneChangeMono : MonoBehaviour
    {
        [SerializeField]
        [Scene]
        private string sceneToLoad;

        public void LoadScene()
        {
            SceneSwitchManager.Instance.SwitchScene(sceneToLoad);
        }
    }
}