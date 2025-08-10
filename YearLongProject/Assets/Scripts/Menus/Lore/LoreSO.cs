using System;
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

        [TextArea(3, 10000)]
        public string Lore;

        public string[] GenerateLorePages()
        {
            string[] pages = Lore.Split("#", StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < pages.Length; i++)
            {
                pages[i] = pages[i].Trim();
            }

            return pages;
        }
    }
}