using System.Collections.Generic;
using GameEntities;
using Hitbox.DataStructures;

namespace Hitbox.System
{
    /// <summary>
    ///     Represents the result of instantiating a hitbox
    /// </summary>
    public struct HitboxInstantiateResult
    {
        public List<HitImpact> HitImpacts;
        public HitboxInstance HitboxInstance;
    }

    /// <summary>
    ///     Represents an impact of a hitbox on a hurtbox
    /// </summary>
    public struct HitImpact
    {
        public Entity HitEntity;
    }
}