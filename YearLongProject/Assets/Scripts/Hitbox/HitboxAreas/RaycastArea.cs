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
    private readonly float _rotation;

    /// <summary>
    ///     Offset from the source position to start the raycast
    /// </summary>
    private readonly Vector2 _startOffset;

    /// <summary>
    ///     Length of the raycast
    /// </summary>
    private readonly float _length;

    public RaycastArea(Vector2 startOffset, float rotation, float length)
    {
        _startOffset = startOffset;
        _rotation = rotation;
        _length = length;
    }

    public Collider2D[] GetCollidersInArea(HitboxContext context)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(
            CalcStartPosition(context),
            CalcDirection(context),
            _length,
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
            CalcDirection(context) * _length,
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
        Vector2 offset = Quaternion.Euler(0, 0, context.SourceAngle) * _startOffset;
        if (context.FlipX)
        {
            offset.x = -offset.x;
        }

        return context.SourcePosition + offset;
    }

    private Vector2 CalcDirection(HitboxContext context)
    {
        float angle = context.SourceAngle + _rotation;
        if (context.FlipX)
        {
            angle = 180 - angle;
        }

        return Quaternion.Euler(0, 0, angle) * Vector2.right;
    }
}