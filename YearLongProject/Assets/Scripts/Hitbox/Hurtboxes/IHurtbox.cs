using Hitbox.DataStructures;
using Hitbox.System;

namespace Hitbox.Hurtboxes
{
    /// <summary>
    ///     Interface for a hurtbox, i.e something that can be hit by a hitbox
    /// </summary>
    public interface IHurtbox
    {
        public void OnHit(HitboxInstance result, HitImpact hitImpact);
    }
}