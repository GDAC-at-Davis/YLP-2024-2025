using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.Generic
{
    /// <summary>
    ///     Logic for a scrollrect with arbitrary text length and multiplayer scrolling
    /// </summary
    public class TextScrollRegion : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect loreScrollView;

        [SerializeField]
        private RectTransform viewContent;

        [SerializeField]
        private TMP_Text textComponent;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private float scrollSpeedLinesPerSecond;

        [SerializeField]
        private float bottomHoldTime;

        [SerializeField]
        private int topBufferSpaceLines;

        private int scrollableLineCount;
        private float bottomHoldTimer;

        private void Update()
        {
            if (scrollableLineCount <= 0)
            {
                return;
            }

            // Automatically scroll the text down
            if (loreScrollView.verticalNormalizedPosition > 0)
            {
                loreScrollView.verticalNormalizedPosition -=
                    scrollSpeedLinesPerSecond / scrollableLineCount * Time.deltaTime;
            }
            else
            {
                // Stop scrolling when it reaches the bottom
                loreScrollView.verticalNormalizedPosition = 0f;

                // Hold for duration before resetting
                if (bottomHoldTimer <= 0f)
                {
                    bottomHoldTimer = bottomHoldTime;
                }
                else
                {
                    bottomHoldTimer -= Time.deltaTime;
                    if (bottomHoldTimer <= 0f)
                    {
                        loreScrollView.verticalNormalizedPosition =
                            1f + (float)topBufferSpaceLines / scrollableLineCount;
                    }
                }
            }
        }

        public void SetVisible(bool visible)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
        }

        public void SetText(string text)
        {
            if (text == null)
            {
                Debug.LogError("Lore Text is not assigned in the TextScrollRegion.");
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                // zero-line text causes issues with rect transform
                text = " ";
            }

            textComponent.text = text;

            int currentLineCount = textComponent.GetTextInfo(text).lineCount;

            float textHeight = textComponent.preferredHeight;

            Vector2 currentSize = viewContent.sizeDelta;
            currentSize.y = textComponent.preferredHeight;
            viewContent.sizeDelta = currentSize;

            float viewportHeight = loreScrollView.viewport.rect.height;

            float scrollableHeight = textHeight - viewportHeight;
            float heightPerLine = textHeight / currentLineCount;

            scrollableLineCount = Mathf.CeilToInt(scrollableHeight / heightPerLine);

            // Reset the scroll position to the top plus buffer
            loreScrollView.verticalNormalizedPosition = 1f + (float)topBufferSpaceLines / currentLineCount;

            bottomHoldTimer = 0f;
        }
    }
}