using CharacterScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button that holds characterSO for character selection
/// </summary>
public class CharacterSelectButton : MonoBehaviour
{
    Button button;

    public CharacterSO Character;

    public void Init(CharacterSO character)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = character.CharacterDisplayName;
        GetComponent<Image>().sprite = character.CharacterPortrait;

        button = GetComponent<Button>();
        Character = character; 
    }
}
