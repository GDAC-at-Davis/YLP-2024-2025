using CharacterScripts;
using Input_Scripts;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBehavior : MonoBehaviour
{
    protected Button button;

    protected void Start()
    {
        button = GetComponent<Button>();
    }

    public abstract void OnClick(PlayerCursorController cursor);
    public abstract void OnHoverEnter(PlayerCursorController cursor);
    public abstract void OnHoverExit(PlayerCursorController cursor);
}