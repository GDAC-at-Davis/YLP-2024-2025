using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // this script right now really should only control the scelect indication behavior and moving between scenes
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Image scelect; // scythe select object. I'll define all of scelects behavior when moving in here
    public Animator animator; // scythe animator controller
    private GameObject lastSelected; // last selected menu item

    public Animator transition_animator; // for fading in and fading out

    void Start()
    {
    }

    // Update is called once per frame
     void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        // Check if the selection has changed
        if (selected != lastSelected)
        {
            lastSelected = selected; // Update the last selected object
            animator.SetTrigger("move");

            // Only move if a valid object is selected
            if (selected != null && selected.TryGetComponent<RectTransform>(out RectTransform buttonRect))
            {
                Vector3 buttonPosition = buttonRect.position;
                scelect.transform.position = buttonPosition + new Vector3(-100, 0, 0);
            }
        }

        // WOOOOOOOO selection checking
        if (selected != null && Input.GetButtonDown("Submit"))
        {
            if (selected.TryGetComponent<Button>(out Button button))
            {
                animator.SetTrigger("select");
            }
        }
    }

    public void live(){
        transition_animator.SetTrigger("exit");
        StartCoroutine(WaitForAnimation("Fade_out", "live"));
    }

    public void lore(){
        // transition to lore scene
    }

    public void leave(){
        transition_animator.SetTrigger("exit");
        StartCoroutine(WaitForAnimation("Fade_out", "exit"));
    }

    // scelect behavior functions
    void scelect_move(){
        
    }

    private IEnumerator WaitForAnimation(string stateName, string outcome){
    // Wait until the animation starts
        yield return new WaitUntil(() => transition_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName));

        // Wait until the animation finishes
        yield return new WaitUntil(() => transition_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        switch(outcome){
            case "exit":
                Application.Quit();
                break;

            case "live":
                SceneManager.LoadScene("NewFighterSelect");
                break;

            case "lore":
                // implement scene switching
                break;
        }
    }

}
