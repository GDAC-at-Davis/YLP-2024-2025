using Animancer;
using UnityEngine;

public class MoveState : CharacterState
{
    [SerializeField]
    private AnimancerComponent animancer;

    private void Update()
    {
        Vector2 moveInput = ActionManager.GetPlayerActionInput().MoveDir;
        MovementController.SetCharacterMove(moveInput.x);
    }

    protected override void OnEnable()
    {
        animancer.Play(StateAnimation);
    }

    protected override void OnDisable()
    {
    }
}