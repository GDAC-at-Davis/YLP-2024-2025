using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace State_Machine_Scripts
{
    [CreateAssetMenu(fileName = "StateName", menuName = "StateNameSO")]
    public class StateNameSO : ScriptableObject
    {
        [BoldHeader("State Name")]
        [InfoBox(
            "A state name. This is used in place of hand-typed strings to avoid typos and make it easier to change state names.")]
        [ResizableTextArea]
        public string StateDescription;

        public string Value => name;

        public static implicit operator string(StateNameSO so)
        {
            return so.name;
        }
    }
}