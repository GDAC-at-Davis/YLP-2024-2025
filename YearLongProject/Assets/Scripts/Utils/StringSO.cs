using Base;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "StringScriptableObject", menuName = "StringSO")]
    public class StringSO : DescriptionSO
    {
        [SerializeField]
        private string value;

        public string Value
        {
            get => value;
            set => this.value = value;
        }
    }
}