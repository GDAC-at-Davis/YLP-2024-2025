using System.Collections.Generic;
using UnityEngine;

namespace Menus.Lore
{
    /// <summary>
    ///     Represents a lore entry in the game for any arbitrary thing (stage or character)
    /// </summary>
    [CreateAssetMenu(fileName = "LoreSO", menuName = "LoreSO")]
    public class LoreSO : ScriptableObject
    {
        public string LoreTitle;

        [TextArea(3, 10)]
        public List<string> LoreText;
    }
}