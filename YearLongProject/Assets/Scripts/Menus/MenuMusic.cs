using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

namespace Menus
{
    /// <summary>
    ///     Menu music should be unbroken across scenes
    /// </summary>
    public class MenuMusic : MonoBehaviour
    {
        [SerializeField]
        [Scene]
        private string gameplayScene;

        [SerializeField]
        private StudioEventEmitter menuMusicEmitter;

        public void OnSceneChange(string sceneName)
        {
            if (sceneName == gameplayScene)
            {
                menuMusicEmitter.Stop();
            }
            else if (!menuMusicEmitter.IsPlaying())
            {
                menuMusicEmitter.Play();
            }
        }
    }
}