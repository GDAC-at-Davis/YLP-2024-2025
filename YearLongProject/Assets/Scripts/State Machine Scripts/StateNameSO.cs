using Base;
using UnityEngine;

namespace State_Machine_Scripts
{
    [CreateAssetMenu(fileName = "StateName", menuName = "StateNameSO")]
    public class StateNameSO : DescriptionSO
    {
        public string Value => name;

        public static implicit operator string(StateNameSO so)
        {
            return so.name;
        }
    }
}