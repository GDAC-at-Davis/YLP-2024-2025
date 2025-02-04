using UnityEngine;
using Hitbox.DataStructures;

public abstract class Entity : MonoBehaviour
{
    private int _entityID;
    public int EntityID { get => _entityID; set => _entityID = value; }

    public virtual void Init(int id)
    {
        EntityID = id;
    }

    public virtual void OnHitByAttack(HitboxInstance hitboxInstance)
    {
    }
}
