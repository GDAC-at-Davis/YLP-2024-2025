using LevelScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button that holds levelSO for level selection
/// </summary>
public class LevelSelectButton : MonoBehaviour
{
    Button button;

    public LevelSO Level;

    public void Init(LevelSO level)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = level.LevelDisplayName;
        GetComponent<Image>().sprite = level.LevelPortrait;

        button = GetComponent<Button>();
        Level = level;
    }
}
