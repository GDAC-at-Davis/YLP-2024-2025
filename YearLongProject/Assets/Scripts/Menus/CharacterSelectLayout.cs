using CharacterScripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// populates character select menu with characters based on characterSO's in characterList
/// </summary>
public class CharacterSelectLayout : MonoBehaviour
{
    [SerializeField]
    private List<CharacterSO> characterList;

    [SerializeField]
    private GameObject buttonTemplate;

    private void Start()
    {
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
        foreach (CharacterSO character in characterList)
        {
            Instantiate(buttonTemplate, gridLayout.transform).GetComponent<CharacterSelectButton>().Init(character);
        }
    }   
}
