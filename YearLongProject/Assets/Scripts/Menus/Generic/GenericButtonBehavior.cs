using Animancer;
using Input_Scripts;

namespace Menus.Generic
{
    /// <summary>
    ///     Generic button behavior that just passes through to unity events
    /// </summary>
    public class GenericButtonBehavior : ButtonBehavior
    {
        public UnityEvent OnClickEvent;

        public UnityEvent OnHoverEnterEvent;

        public UnityEvent OnHoverExitEvent;

        public override void OnClick(PlayerCursorController cursor)
        {
            OnClickEvent?.Invoke();
        }

        public override void OnHoverEnter(PlayerCursorController cursor)
        {
            OnHoverEnterEvent?.Invoke();
        }

        public override void OnHoverExit(PlayerCursorController cursor)
        {
            OnHoverExitEvent?.Invoke();
        }
    }
}