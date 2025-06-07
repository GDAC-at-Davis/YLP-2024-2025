using Input_Scripts;
using Managers;
using Menus;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class FightButton : ButtonBehavior
{
    [SerializeField]
    private GameDataSO gameDataSO;

    [SerializeField]
    [Scene]
    private string levelSelect;

    [SerializeField]
    private Sprite fight;

    [SerializeField]
    private Sprite hoverFight;

    private Image currentFight;

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