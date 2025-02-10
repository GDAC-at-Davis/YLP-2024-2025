using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    private bool slow;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            slow = !slow;
            if (slow)
            {
                Time.timeScale = 0.02f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}