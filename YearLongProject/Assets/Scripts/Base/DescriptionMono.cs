using UnityEngine;

namespace Base
{
    /// <summary>
    ///     MonoBehaviour with a description
    /// </summary>
    public class DescriptionMono : MonoBehaviour
    {
        [SerializeField]
        [TextArea]
        private string devDescription;
    }
}