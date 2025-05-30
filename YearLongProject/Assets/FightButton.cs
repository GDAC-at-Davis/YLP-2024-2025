using Input_Scripts;
using Managers;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FightButton : ButtonBehavior
{
    [SerializeField]
    GameDataSO gameDataSO;

    [SerializeField]
    [Scene]
    string levelSelect;

    [SerializeField]
    Sprite fight;
    [SerializeField]
    Sprite hoverFight;

    Image currentFight;

    protected override void Start()
    {
        base.Start();
        gameDataSO.OnAllPlayersUnready += UnreadyUp;
        gameDataSO.OnAllPlayersReady += ReadyUp;
        gameObject.SetActive(false);

        currentFight = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        gameDataSO.OnAllPlayersUnready -= UnreadyUp;
        gameDataSO.OnAllPlayersReady -= ReadyUp;
    }

    private void UnreadyUp()
    {
        gameObject.SetActive(false);
    }

    private void ReadyUp()
    {
        gameObject.SetActive(true);
        currentFight.sprite = fight;
    }

    public override void OnClick(PlayerCursorController cursor)
    {
        gameDataSO.LoadScene(levelSelect);
    }

    public override void OnHoverEnter(PlayerCursorController cursor)
    {
        currentFight.sprite = hoverFight;
    }

    public override void OnHoverExit(PlayerCursorController cursor)
    {
        currentFight.sprite = fight;
    }
}
