using Base;
using UnityEngine;

namespace CharacterScripts
{
    public class FlipX3D : DescriptionMono
    {
        [SerializeField]
        private Transform visualObject;

        public void SetFlipX(bool flipped)
        {
            if (visualObject == null)
            {
                return;
            }

            Vector3 localScale = visualObject.localScale;
            localScale.x = flipped ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
            visualObject.localScale = localScale;
        }
    }
}