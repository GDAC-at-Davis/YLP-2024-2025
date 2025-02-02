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
        private Character _character;

        [SerializeField]
        private Transform _hitboxSourceTransform;

        [SerializeField]
        private bool _flipX;

        [SerializeField]
        private LayerMask _hitboxLayerMask;

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
                SourceCharacter = _character,
                SourcePosition = _hitboxSourceTransform.position,
                SourceAngle = _hitboxSourceTransform.eulerAngles.z,
                LayerMask = _hitboxLayerMask,
                FlipX = _flipX
            };
        }
    }
}