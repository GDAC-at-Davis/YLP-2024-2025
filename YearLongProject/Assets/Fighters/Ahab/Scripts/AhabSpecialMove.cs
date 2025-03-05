using EditorUtils.BoldHeader;
using NaughtyAttributes;
using UnityEngine;

namespace Fighters.Ahab.Scripts
{
    /// <summary>
    ///     Handles logic for throwing the SHARKSON.
    /// </summary>
    public class AhabSpecialMove : MonoBehaviour
    {
        [BoldHeader("Ahab Special Move Script")]
        [InfoBox("Handles throwing the SHARKSON.")]
        [SerializeField]
        public AhabSharkson sharkson;

        [SerializeField]
        public Transform throwTransform;

        [SerializeField]
        private float throwForce;

        public void ThrowSharkSon()
        {
            sharkson.gameObject.transform.SetPositionAndRotation(throwTransform.position, throwTransform.rotation);
            sharkson.Throw(throwForce);
        }
    }
}