using System.Collections.Generic;
using Hitbox.DataStructures;
using Hitbox.HitboxAreas;
using UnityEngine;

namespace Hitbox.Emitters
{
    /// <summary>
    ///     Emits hitboxes
    /// </summary>
    public class HitboxEmitter : MonoBehaviour
    {
        [Header("Depends")]

        [SerializeField]
        private HitboxSystemSo hitboxSystemSo;

        [SerializeField]
        private Entity entity;

        [SerializeField]
        private Transform hitboxSourceTransform;

        [SerializeField]
        private bool flipX;

        [SerializeField]
        private LayerMask hitboxLayerMask;

        [SerializeField]
        private string hitboxID;

        public Entity Entity => entity;

        // prevents hitbox from repeatedly hitting hurtboxes
        public HashSet<Entity> HitEntities = new();

        public void EmitHitbox(IHitboxArea hitboxArea, HitboxEffect hitboxEffect)
        {
            HitboxContext context = GetContext();

            hitboxSystemSo.InstantiateHitbox(new HitboxInstance
            {
                HitboxArea = hitboxArea,
                Context = context,
                HitboxEffect = hitboxEffect
            });
        }

        public HitboxContext GetContext()
        {
            return new HitboxContext
            {
                Source = this,
                SourcePosition = hitboxSourceTransform.position,
                SourceAngle = hitboxSourceTransform.eulerAngles.z,
                LayerMask = hitboxLayerMask,
                FlipX = flipX,
                HitboxID = hitboxID
            };
        }

        public void SetFlipX(bool flipX)
        {
            this.flipX = flipX;
        }
    }
}