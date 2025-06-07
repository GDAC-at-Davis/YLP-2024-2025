using Input_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Menus.Lore
{
    /// <summary>
    ///     Button representing a lore entry that can be viewed
    /// </summary>
    public class LoreSelectButton : ButtonBehavior
    {
        [SerializeField]
        private Image loreImage;

        [SerializeField]
        private TMP_Text loreHeaderText;

        public UnityEvent<LoreSO> OnLoreSelected;

        public UnityEvent OnHovered;
        public UnityEvent OnUnhovered;

        private LoreSO lore;

        public void Initialize(LoreSO lore, string header, Sprite sprite)
        {
            this.lore = lore;
            loreHeaderText.text = header;
            loreImage.sprite = sprite;
        }

        public override void OnClick(PlayerCursorController cursor)
        {
            if (lore == null)
            {
                Debug.Log("LoreSO is not initialized for LoreSelectButton.");
                return;
            }

            OnLoreSelected?.Invoke(lore);
        }

        public override void OnHoverEnter(PlayerCursorController cursor)
        {
            OnHovered?.Invoke();
        }

        public override void OnHoverExit(PlayerCursorController cursor)
        {
            OnUnhovered?.Invoke();
        }
    }
}