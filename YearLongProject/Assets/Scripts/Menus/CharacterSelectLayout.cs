using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    /// <summary>
    ///     populates character select menu with characters based on characterSO's in characterList
    /// </summary>
    public class CharacterSelectLayout : MonoBehaviour
    {
        [SerializeField]
        private CharacterSelectRoster characterList;

        [SerializeField]
        private GameObject buttonTemplate;

        private void Start()
        {
            var gridLayout = GetComponent<GridLayoutGroup>();
            foreach (CharacterSelectRoster.CharacterSelectData character in characterList.Characters)
            {
                if (character.IsHidden)
                {
                    continue;
                }

                Instantiate(buttonTemplate, gridLayout.transform).GetComponent<CharacterSelectButton>()
                    .Init(character.Character);
            }
        }
    }
}