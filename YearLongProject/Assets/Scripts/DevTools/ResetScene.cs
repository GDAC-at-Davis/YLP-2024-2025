using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}