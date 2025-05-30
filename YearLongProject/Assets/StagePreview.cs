using LevelScripts;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StagePreview : MonoBehaviour
{
    [SerializeField]
    private GameDataSO gameDataSO;

    private Image displayImage;
    private TextMeshProUGUI text;

    private void Start()
    {
        displayImage = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        gameDataSO.OnProspectLevelChanged += HandleProspectLevelChanged;
        gameDataSO.OnSelectedLevelChanged += HandleSelectedLevelChanged;
    }

    private void OnDestroy()
    {
        gameDataSO.OnProspectLevelChanged -= HandleProspectLevelChanged;
        gameDataSO.OnSelectedLevelChanged -= HandleSelectedLevelChanged;
    }

    private void HandleSelectedLevelChanged(LevelSO level)
    {
        displayImage.sprite = null;
        displayImage.color = new Color(1, 1, 1, 0.5f);
        text.text = "";

        if (level == null)
        {
            return;
        }

        displayImage.sprite = level.LevelPreview;
        displayImage.color = new Color(1, 1, 1, 1);
        text.text = level.LevelDisplayName;
    }

    private void HandleProspectLevelChanged(LevelSO level)
    {
        displayImage.sprite = null;
        displayImage.color = new Color(0, 0, 0, 0.5f);
        text.text = "";

        if (level == null)
        {
            return;
        }

        displayImage.sprite = level.LevelPreview;
        displayImage.color = new Color(1, 1, 1, 0.5f);
        text.text = level.LevelDisplayName;
    }
}
