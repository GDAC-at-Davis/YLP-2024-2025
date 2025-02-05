using Hitbox.DataStructures;
using Hitbox.HitboxAreas;
using UnityEngine;

/// <summary>
///     Area for a raycast hitbox
/// </summary>
public class RaycastArea : IHitboxArea
{
    /// <summary>
    ///     Rotation of the raycast direction in degrees. 0 goes right
    /// </summary>
    private readonly float rotation;

    /// <summary>
    ///     Offset from the source position to start the raycast
    /// </summary>
    private readonly Vector2 startOffset;

    /// <summary>
    ///     Length of the raycast
    /// </summary>
    private readonly float length;

    public RaycastArea(Vector2 startOffset, float rotation, float length)
    {
        this.startOffset = startOffset;
        this.rotation = rotation;
        this.length = length;
    }

    public Collider2D[] GetCollidersInArea(HitboxContext context)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(
            CalcStartPosition(context),
            CalcDirection(context),
            length,
            context.LayerMask);

        var colliders = new Collider2D[hit.Length];

        for (var i = 0; i < hit.Length; i++)
        {
            colliders[i] = hit[i].collider;
        }

        return colliders;
    }

    public void DrawAreaDebug(HitboxContext context, DrawDebugConfig config)
    {
        Debug.DrawRay(CalcStartPosition(context),
            CalcDirection(context) * length,
            config.Color,
            config.Duration);
    }

    /// <summary>
    ///     Start position of the raycast
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private Vector2 CalcStartPosition(HitboxContext context)
    {
        Vector2 offset = Quaternion.Euler(0, 0, context.SourceAngle) * startOffset;
        if (context.FlipX)
        {
            offset.x = -offset.x;
        }

        return context.SourcePosition + offset;
    }

    private Vector2 CalcDirection(HitboxContext context)
    {
        float angle = context.SourceAngle + rotation;
        if (context.FlipX)
        {
            angle = 180 - angle;
        }

        return Quaternion.Euler(0, 0, angle) * Vector2.right;
    }
}