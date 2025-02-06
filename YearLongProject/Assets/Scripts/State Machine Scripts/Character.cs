using Animancer;
using GameEntities;
using Hitbox.DataStructures;
using UnityEngine;

public class Character : Entity
{
    [SerializeField]
    public CharacterActionManager ActionManager;

    [SerializeField]
    public CharacterMovementController MovementController;

    /// <summary>
    ///     Id of the actual player. Used for input and other player specific things.
    /// </summary>
    public int PlayerId => playerId;

    private int playerId = -1;

    protected bool IsInvincible;

#if UNITY_EDITOR
    private void OnValidate()
    {
        gameObject.GetComponentInParentOrChildren(ref ActionManager);
        gameObject.GetComponentInParentOrChildren(ref MovementController);
    }
#endif

    public void Init(int id)
    {
        playerId = id;
        transform.parent = null;

        ActionManager.Init();
    }

    // Callback for this Character being hit by an attack
    // Will route calls to health/stats manager, action manager
    // Example of override: reflecting damage back at attacker
    public override void OnHitByAttack(HitboxInstance hitboxInstance)
    {
        if (IsInvincible)
        {
        }
    }

    // Callback for landing an attack on a Character
    // Example of override: granting this Character buffs on landing hit
    public virtual void OnAttackHit(Character other)
    {
    }

    public virtual void SetIsInvincible(bool isInvincible)
    {
        IsInvincible = isInvincible;
    }

    public virtual void Die()
    {
        //Temporary implementation
        Destroy(gameObject);
    }
}