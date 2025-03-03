using Base;
using UnityEngine;

namespace CharacterScripts
{
    /// <summary>
    /// Scriptable Object that holds relevant character information for reference
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
    public class CharacterSO : ScriptableObject
    {
        [Tooltip("Character Name")]
        public string CharacterDisplayName;

        [Tooltip("Reference for character's gameobject")]
        public GameObject CharacterPrefab;

        [Tooltip("Character portrait for use in UI Elements")]
        public Sprite CharacterPortrait;
    }
}
