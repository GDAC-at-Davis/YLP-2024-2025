using UnityEngine;
using UnityEngine.Events;

namespace Fighters.Ahab.Scripts
{
    public class AhabStateOverrideManager : MonoBehaviour
    {
        [SerializeField]
        private AhabSharkson sharksonScript;

        public bool sharksonThrown => sharksonScript.thrown;

        public UnityEvent AhabHeavyAttack;
        public UnityEvent SharkHeavyAttack;
        public UnityEvent AhabSpecialAttack;
        public UnityEvent SharkSpecialAttack;

        public void HeavyPressed()
        {
            if (sharksonThrown) SharkHeavyAttack.Invoke();
            else AhabHeavyAttack.Invoke();
        }

        public void SpecialPressed()
        {
            if (sharksonThrown) SharkSpecialAttack.Invoke();
            else AhabSpecialAttack.Invoke();
        }
    }
}