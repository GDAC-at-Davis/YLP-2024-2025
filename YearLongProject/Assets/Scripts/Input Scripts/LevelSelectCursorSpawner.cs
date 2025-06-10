using Input_Scripts;
using Managers;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
///     Spawns a cursor for player 1 to select the stage
/// </summary>
public class LevelSelectCursorSpawner : MonoBehaviour
{
    [Header("Cursor")]

    [SerializeField]
    private GameDataSO gameDataSO;

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

    private void Start()
    {
        PlayerCursorController cursor = Instantiate(cursorPrefab, container);
        int lowestPlayerId = gameDataSO.GetLowestPlayerId();

        if (lowestPlayerId == -1)
        {
            Debug.LogError("No players found in game data. Cannot spawn cursor.");
            return;
        }

        cursor.Initialize(lowestPlayerId, cursorBottomLeft, cursorTopRight, container);
        cursor.BackAction = BackToCharSelect;
    }

    private void BackToCharSelect(PlayerCursorController cursor)
    {
        gameDataSO.ClearPlayerData();
        gameDataSO.LoadScene(charSelectScene);
    }
}