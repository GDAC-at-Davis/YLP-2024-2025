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
        private HitboxSystemSO _hitboxSystemSo;

        [SerializeField]
        private Entity _entity;

        [SerializeField]
        private Transform _hitboxSourceTransform;

        [SerializeField]
        private bool _flipX;

        [SerializeField]
        private LayerMask _hitboxLayerMask;

        [SerializeField]
        private string _hitboxID;

        // prevents hitbox from repeatedly hitting hurtboxes
        public HashSet<Entity> HitEntities = new();
        public Entity Entity => _entity;

        public void EmitHitbox(IHitboxArea hitboxArea, HitboxEffect hitboxEffect)
        {
            HitboxContext context = GetContext();

            _hitboxSystemSo.InstantiateHitbox(new HitboxInstance
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
                SourcePosition = _hitboxSourceTransform.position,
                SourceAngle = _hitboxSourceTransform.eulerAngles.z,
                LayerMask = _hitboxLayerMask,
                FlipX = _flipX,
                HitboxID = _hitboxID
            };
        }

        public void SetFlipX(bool flipX)
        {
            _flipX = flipX;
        }
    }
}