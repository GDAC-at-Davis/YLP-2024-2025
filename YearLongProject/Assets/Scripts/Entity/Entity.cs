using Base;
using Hitbox.DataStructures;
using UnityEngine;

public abstract class Entity : DescriptionMono
{
    [SerializeField]
    private int entityID;

    public int EntityID
    {
        get => entityID;
        set => entityID = value;
    }

    public virtual void Init(int id)
    {
        EntityID = id;
    }

    public virtual void OnHitByAttack(HitboxInstance hitboxInstance)
    {
    }
}