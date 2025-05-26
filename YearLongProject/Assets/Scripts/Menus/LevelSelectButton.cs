using Input_Scripts;
using LevelScripts;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button that holds levelSO for level selection
/// </summary>
public class LevelSelectButton : ButtonBehavior
{
    [SerializeField]
    GameDataSO gameDataSO;

    [SerializeField]
    [Scene]
    private string gameplayScene;

    public LevelSO Level;

    public void Init(LevelSO level)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = level.LevelDisplayName;
        GetComponent<Image>().sprite = level.LevelPortrait;

        button = GetComponent<Button>();
        Level = level;
    }

    public override void OnClick(PlayerCursorController cursor)
    {
        cursor.transform.position = transform.position;
        cursor.SetText("");
        cursor.Selected = true;

        gameDataSO.SetSelectedLevel(Level);
        gameDataSO.LoadScene(gameplayScene);
    }

    public override void OnHoverEnter(PlayerCursorController cursor)
    {
        throw new System.NotImplementedException();
    }

    public override void OnHoverExit(PlayerCursorController cursor)
    {
        throw new System.NotImplementedException();
    }
}
