using CharacterScripts;
using EditorUtils.BoldHeader;
using Hitbox.System;
using NaughtyAttributes;
using UnityEngine;

namespace Camera
{
    /// <summary>
    ///     Script for emitting screen shake events on an entity
    /// </summary>
    public class EntityScreenShakeEmitter : MonoBehaviour
    {
        [BoldHeader("Screen Shake Emitter")]
        [InfoBox("Handles emitting screen shake effects. Don't remove!", EInfoBoxType.Warning)]
        [Header("Dependencies")]

        [SerializeField]
        private CharacterFacingDirection characterFacingDirection;

        public void ShakeOnLandHit(HitboxInstantiateResult hitboxInstantiateResult)
        {
            ScreenShakeEffect effect = hitboxInstantiateResult.HitboxInstance.HitboxEffect.ScreenShakeEffect;

            Vector2 velocity = effect.Velocity;

            // Flip the X velocity if the effect is set to do so and a FlipXHandler is provided
            if (effect.FlipXVelocity && characterFacingDirection != null)
            {
                velocity.x *= characterFacingDirection.CurrentFlipX ? -1 : 1;
            }

            // Use the main camera's position as the source to avoid any falloff from distance; this keeps things simple
            UnityEngine.Camera cam = UnityEngine.Camera.main;
            if (cam != null)
            {
                effect.ImpulseDefinition.CreateEvent(cam.transform.position, velocity);
            }
        }
    }
}