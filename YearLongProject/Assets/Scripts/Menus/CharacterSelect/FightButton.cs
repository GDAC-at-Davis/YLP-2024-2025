using Animancer;
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

    [Header("Events")]

    [SerializeField]
    private UnityEvent onFightSelected;

    [SerializeField]
    private UnityEvent onReady;

    [SerializeField]
    private UnityEvent onUnready;

    [SerializeField]
    private UnityEvent onHovered;

    [SerializeField]
    private UnityEvent onUnhovered;

    private Image currentFight;

    private int hovering;

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
        onUnready?.Invoke();
    }

    private void ReadyUp()
    {
        gameObject.SetActive(true);
        currentFight.sprite = fight;
        onReady?.Invoke();
    }

    public override void OnClick(PlayerCursorController cursor)
    {
        gameDataSO.LoadScene(levelSelect);
        onFightSelected?.Invoke();
    }

    public override void OnHoverEnter(PlayerCursorController cursor)
    {
        currentFight.sprite = hoverFight;
        hovering++;

        if (hovering > 1)
        {
            return;
        }

        onHovered?.Invoke();
    }

    public override void OnHoverExit(PlayerCursorController cursor)
    {
        hovering--;

        if (hovering > 0)
        {
            return;
        }

        currentFight.sprite = fight;
        onUnhovered?.Invoke();
    }
}