using Base;
using Hitbox.DataStructures;
using Hitbox.Hurtboxes;
using UnityEngine;

namespace Hitbox
{
    /// <summary>
    ///     Represents a character hurtbox. Needs to be on the same gameobject as the collider
    /// </summary>
    public class CharacterHurtbox : DescriptionMono, IHurtbox
    {
        [SerializeField]
        private Character _attachedCharacter;

        public Character AttachedCharacter => _attachedCharacter;

        public void OnHit(HitboxInstance hitboxInstance)
        {
            if (_attachedCharacter == null)
            {
                return;
            }

            _attachedCharacter.OnHitByAttack(hitboxInstance);
        }
    }
}