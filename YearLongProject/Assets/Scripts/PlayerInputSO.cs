using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "Scriptable Objects/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject
{
    public UnityAction<int, Vector2> MoveEvent;
    public UnityAction<int, bool> AttackEvent;
    public UnityAction<int, bool> JumpEvent;
}
