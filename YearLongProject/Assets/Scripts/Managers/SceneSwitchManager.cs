using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneSwitchManager : MonoBehaviour
    {
        public Animator transition_animator; // for fading in and fading out
        public static SceneSwitchManager Instance { get; private set; }

        private bool isTransitioning;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SwitchScene(string scene)
        {
            if (isTransitioning)
            {
                Debug.LogWarning("Scene switch already in progress. Ignoring request.");
                return;
            }

            isTransitioning = true;
            StartCoroutine(SwitchSceneCorout(scene));
        }

        private IEnumerator SwitchSceneCorout(string targetScene)
        {
            transition_animator.Play("Fade_out", 0, 0f);

            yield return new WaitForEndOfFrame();

            // Wait until the animation finishes
            yield return new WaitUntil(() => transition_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
            yield return new WaitUntil(() => op.isDone);

            // Fade in
            transition_animator.Play("Fade_in", 0, 0f);

            yield return new WaitForEndOfFrame();

            yield return new WaitUntil(() => transition_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            isTransitioning = false;
        }
    }
}