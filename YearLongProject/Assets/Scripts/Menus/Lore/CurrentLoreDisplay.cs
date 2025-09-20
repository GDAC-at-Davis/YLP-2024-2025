using TMPro;
using UnityEngine;

namespace Menus.Lore
{
    public class CurrentLoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text loreContentText;

        [SerializeField]
        private TMP_Text loreHeaderText;

        [SerializeField]
        private TMP_Text pageNumberText;

        private LoreSO currentLore;
        private int currentPage;

        private string[] lorePages;

        public void SetLore(LoreSO lore)
        {
            if (lore == null)
            {
                Debug.LogWarning("LoreSO is null. Cannot set lore content.");
                return;
            }

            currentLore = lore;
            currentPage = 0;
            lorePages = lore.GenerateLorePages();
            DisplayLorePage(0);
            loreHeaderText.text = lore.LoreTitle;
        }

        private void DisplayLorePage(int page)
        {
            if (currentLore == null)
            {
                Debug.LogWarning("Current lore is not set. Cannot display lore page.");
                return;
            }

            if (lorePages == null || lorePages.Length == 0)
            {
                Debug.LogWarning("Lore text is empty or not set.");
                loreContentText.text = "No lore available.";
                return;
            }

            if (page < 0 || page >= lorePages.Length)
            {
                return;
            }

            currentPage = page;
            loreContentText.text = lorePages[page];

            pageNumberText.text = $"{currentPage + 1}/{lorePages.Length}";
        }

        public void IncrementLorePage()
        {
            DisplayLorePage(currentPage + 1);
        }

        public void DecrementLorePage()
        {
            DisplayLorePage(currentPage - 1);
        }
    }
}