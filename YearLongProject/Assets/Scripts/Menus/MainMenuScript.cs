using Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    // this script right now really should only control the scelect indication behavior and moving between scenes
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Image scelect; // scythe select object. I'll define all of scelects behavior when moving in here
    public Animator animator; // scythe animator controller

    [Scene]
    public string liveScene; // the scene to load when the player selects the "live" button

    [Scene]
    public string loreScene;

    private GameObject lastSelected; // last selected menu item

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        // Check if the selection has changed
        if (selected != lastSelected)
        {
            lastSelected = selected; // Update the last selected object
            animator.SetTrigger("move");

            // Only move if a valid object is selected
            if (selected != null && selected.TryGetComponent(out RectTransform buttonRect))
            {
                Vector3 buttonPosition = buttonRect.position;
                scelect.transform.position = buttonPosition + new Vector3(-100, 0, 0);
            }
        }

        // WOOOOOOOO selection checking
        if (selected != null && Input.GetButtonDown("Submit"))
        {
            if (selected.TryGetComponent(out Button button))
            {
                animator.SetTrigger("select");
            }
        }
    }

    public void live()
    {
        SceneSwitchManager.Instance.SwitchScene(liveScene);
    }

    public void lore()
    {
        SceneSwitchManager.Instance.SwitchScene(loreScene);
    }

    public void leave()
    {
        Application.Quit();
    }

    // scelect behavior functions
    private void scelect_move()
    {
    }
}