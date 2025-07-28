using Movement;
using UnityEngine;

namespace State_Machine_Scripts.Modifiers
{
    public class WizardSpecialStateSwitcher : MonoBehaviour
    {
        [SerializeField]
        ComboStateSwitcher comboStateSwitcher;

        [SerializeField]
        CharacterActionManager actionManager;

        [SerializeField]
        CharacterState state;

        [SerializeField]
        SimpleMovementController controller;

        public void OnSpecialInput()
        {
            if (controller.VerticalInput > 0 && comboStateSwitcher.CurrentComboIndex == 0)
            {
                actionManager.SetState(state);   
                return;
            }
            comboStateSwitcher.TryCombo();
        }
    }
}
