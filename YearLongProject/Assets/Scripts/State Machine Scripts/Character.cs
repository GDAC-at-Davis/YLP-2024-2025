using Animancer;
using Hitbox.DataStructures;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    public CharacterActionManager actionManager;

    [SerializeField]
    public CharacterMovementController movementController;

    protected bool isInvincible;

#if UNITY_EDITOR
    private void OnValidate()
    {
        gameObject.GetComponentInParentOrChildren(ref actionManager);
        gameObject.GetComponentInParentOrChildren(ref movementController);
    }
#endif

    // Callback for this Character being hit by an attack
    // Will route calls to health/stats manager, action manager
    // Example of override: reflecting damage back at attacker
    public virtual void OnHitByAttack(HitboxInstance hitboxInstance)
    {
        if (isInvincible)
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
        this.isInvincible = isInvincible;
    }

    public virtual void Die()
    {
        //Temporary implementation
        Destroy(gameObject);
    }
}