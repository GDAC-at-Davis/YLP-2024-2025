using System;
using Base;
using Hitbox.DataStructures;
using Hitbox.Emitters;
using Hitbox.HitboxAreas;
using UnityEngine;

namespace Hitbox.Testing
{
    /// <summary>
    ///     Scuffed debug script to test hitboxes
    /// </summary>
    public class HitboxEmitterTester : DescriptionMono
    {
        public enum AreaTypes
        {
            Box,
            Raycast
        }

        [Header("Context")]

        [SerializeField]
        private AreaTypes areaType;

        [SerializeField]
        private HitboxEmitter hitboxEmitter;

        [Header("Box Area")]

        [SerializeField]
        private Vector2 boxCenter;

        [SerializeField]
        private float boxRotation;

        [SerializeField]
        private Vector2 boxSize;

        [Header("Raycast Area")]

        [SerializeField]
        private Vector2 raycastOrigin;

        [SerializeField]
        private float raycastAngle;

        [SerializeField]
        private float raycastDistance;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed");

                IHitboxArea area = null;
                switch (areaType)
                {
                    case AreaTypes.Box:
                        area = new BoxArea(boxCenter, boxRotation, boxSize);
                        break;
                    case AreaTypes.Raycast:
                        area = new RaycastArea(raycastOrigin, raycastAngle, raycastDistance);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                hitboxEmitter.EmitHitbox(area, new HitboxEffect());
            }
        }

        private void OnDrawGizmos()
        {
            HitboxContext context = hitboxEmitter.GetContext();
            IHitboxArea area = null;

            switch (areaType)
            {
                case AreaTypes.Box:
                    area = new BoxArea(boxCenter, boxRotation, boxSize);
                    break;
                case AreaTypes.Raycast:
                    area = new RaycastArea(raycastOrigin, raycastAngle, raycastDistance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            area.DrawAreaDebug(context, new DrawDebugConfig(Color.green, 0f));
        }
    }
}