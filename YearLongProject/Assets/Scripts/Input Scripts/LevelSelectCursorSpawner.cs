using Input_Scripts;
using Managers;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Spawns a cursor for player 1 to select the stage
/// </summary>
public class LevelSelectCursorSpawner : MonoBehaviour
{
    [Header("Cursor")]

    [SerializeField]
    GameDataSO gameDataSO;

    [SerializeField]
    private PlayerCursorController cursorPrefab;

    [SerializeField]
    private RectTransform container;

    [SerializeField]
    private RectTransform cursorBottomLeft;

    [SerializeField]
    private RectTransform cursorTopRight;

    [SerializeField]
    [Scene]
    private string charSelectScene;

    void Start()
    {
        PlayerCursorController cursor = Instantiate(cursorPrefab, container);
        cursor.Initialize(0, cursorBottomLeft, cursorTopRight, container);
        cursor.BackAction = BackToCharSelect;
    }

    void BackToCharSelect(PlayerCursorController cursor)
    {
        gameDataSO.ClearPlayerData();
        gameDataSO.LoadScene(charSelectScene);
    }
}
