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
        [Header("Dependencies")]

        [SerializeField]
        public AhabSharkson sharkson;

        [SerializeField]
        public Transform throwTransform;

        [Header("Config")]

        [SerializeField]
        private Vector2 throwPointOffset;

        [SerializeField]
        private float throwForce;

        private int facingDirection = 1;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)throwTransform.position + throwPointOffset, 0.1f);
        }

        /// <summary>
        ///     Set the facing direction of the SHARKSON. Should be a listener to the Facing Direction script events
        /// </summary>
        /// <param name="flipped"></param>
        public void SetFacing(bool flipped)
        {
            facingDirection = flipped ? -1 : 1;
        }

        public void ThrowSharkSon()
        {
            Vector2 offset = throwPointOffset;
            offset.x *= facingDirection;
            sharkson.Throw((Vector2)throwTransform.position + offset,
                throwTransform.rotation,
                throwForce * facingDirection);
        }
    }
}