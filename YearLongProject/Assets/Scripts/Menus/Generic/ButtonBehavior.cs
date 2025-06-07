using Input_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ButtonBehavior : MonoBehaviour
    {
        protected Button button;
        protected Collider2D col;

        protected virtual void Start()
        {
            button = GetComponent<Button>();
            col = GetComponent<Collider2D>();
        }

        public abstract void OnClick(PlayerCursorController cursor);
        public abstract void OnHoverEnter(PlayerCursorController cursor);
        public abstract void OnHoverExit(PlayerCursorController cursor);
    }
}