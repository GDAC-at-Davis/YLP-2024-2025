using State_Machine_Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Fighters.Ahab.Scripts
{
    public class AhabStateOverrideManager : MonoBehaviour
    {
        [SerializeField]
        private AhabSharkson sharksonScript;

        [SerializeField]
        CharacterActionManager action;

        public bool sharksonThrown => sharksonScript.thrown;
        public bool inSpecial => action.CurrentState.StateName == "AhabSpecial";

        public UnityEvent AhabHeavyAttack;
        public UnityEvent SharkHeavyAttack;
        public UnityEvent AhabSpecialAttack;
        public UnityEvent SharkSpecialAttack;

        public void HeavyPressed()
        {
            Debug.Log(inSpecial);
            if (sharksonThrown) SharkHeavyAttack.Invoke();
            else if (!inSpecial) AhabHeavyAttack.Invoke();
        }

        public void SpecialPressed()
        {
            if (sharksonThrown) SharkSpecialAttack.Invoke();
            else if (!inSpecial) AhabSpecialAttack.Invoke();
        }
    }
}