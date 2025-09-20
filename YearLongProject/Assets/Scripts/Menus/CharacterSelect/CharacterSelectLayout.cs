using UnityEngine;

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
        private CharacterSelectButton buttonTemplate;

        private void Start()
        {
            foreach (CharacterSelectRoster.CharacterSelectData character in characterList.Characters)
            {
                if (character.IsHidden)
                {
                    continue;
                }

                CharacterSelectButton createdButton = Instantiate(buttonTemplate, transform);
                createdButton.Init(character.Character);
                createdButton.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Random.Range(-2f, 2f));
            }
        }
    }
}