using System.Collections;
using UnityEngine;
using Animancer;
using Animancer.FSM;


public class Character : MonoBehaviour
{
    [SerializeField]
    public CharacterActionManager actionManager;

    [SerializeField]
    public CharacterMovementController movementController;

    protected bool isInvincible;

    public int playerId = -1;

    public void Init(int id)
    {
        playerId = id;
        transform.parent = null;

        actionManager.Init();
    }

    // Callback for this Character being hit by an attack
    // Will route calls to health/stats manager, action manager
    // Example of override: reflecting damage back at attacker
    public virtual void OnHitByAttack(Character other)
    {
        if (isInvincible) return;
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
        Destroy(this.gameObject);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        gameObject.GetComponentInParentOrChildren(ref actionManager);
        gameObject.GetComponentInParentOrChildren(ref movementController);
    }
#endif
}
