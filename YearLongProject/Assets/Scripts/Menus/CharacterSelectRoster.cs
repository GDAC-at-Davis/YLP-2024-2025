using System;
using System.Collections.Generic;
using System.Linq;
using CharacterScripts;
using NaughtyAttributes;
using Timeline.Samples;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Menus
{
    [CreateAssetMenu(fileName = "CharacterSelectRoster", menuName = "Character Select Roster")]
    public class CharacterSelectRoster : ScriptableObject
    {
        [Serializable]
        public struct CharacterSelectData
        {
            public CharacterSO Character;
            public bool IsHidden;
        }

        [NoFoldOut]
        public List<CharacterSelectData> Characters;

#if UNITY_EDITOR
        /// <summary>
        ///     Search assets for CharacterSOs and add them to the roster
        /// </summary>
        [Button("Autodetect Characters")]
        public void AutodetectCharacters()
        {
            string[] assets = AssetDatabase.FindAssets("t:CharacterSO");
            IEnumerable<CharacterSO> charactersSOs = assets.Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<CharacterSO>);

            foreach (CharacterSO foundCharacter in charactersSOs)
            {
                var found = false;
                foreach (CharacterSelectData charSelect in Characters)
                {
                    if (charSelect.Character == foundCharacter)
                    {
                        found = true;
                        break;
                    }
                }

                // Found a character not in the roster, add it in
                if (!found)
                {
                    Characters.Add(new CharacterSelectData
                    {
                        Character = foundCharacter,
                        IsHidden = false
                    });
                }
            }
        }
#endif
    }
}