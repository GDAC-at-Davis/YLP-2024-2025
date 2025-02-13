using UnityEngine;

namespace Base
{
    /// <summary>
    ///     ScriptableObject with a description
    /// </summary>
    public class DescriptionSO : ScriptableObject
    {
        [SerializeField]
        [TextArea]
        private string devDescription;
    }
}