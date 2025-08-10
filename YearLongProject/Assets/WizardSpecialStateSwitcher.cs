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
        float sensitivity = 0.8f;

        [SerializeField]
        SimpleMovementController controller;

        public void OnSpecialInput()
        {
            if (controller.VerticalInput > sensitivity && comboStateSwitcher.CurrentComboIndex == 0)
            {
                actionManager.SetState(state);   
                return;
            }
            comboStateSwitcher.TryCombo();
        }
    }
}
