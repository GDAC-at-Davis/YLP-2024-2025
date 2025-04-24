using Input_Scripts;
using UnityEngine;

/// <summary>
/// Spawns a cursor for player 1 to select the stage
/// </summary>
public class LevelSelectCursorSpawner : MonoBehaviour
{
    [Header("Cursor")]

    [SerializeField]
    private PlayerCursorController cursorPrefab;

    [SerializeField]
    private RectTransform container;

    [SerializeField]
    private RectTransform cursorBottomLeft;

    [SerializeField]
    private RectTransform cursorTopRight;

    void Start()
    {
        PlayerCursorController cursor = Instantiate(cursorPrefab, container);
        cursor.Initialize(0, cursorBottomLeft, cursorTopRight);
    }
}
