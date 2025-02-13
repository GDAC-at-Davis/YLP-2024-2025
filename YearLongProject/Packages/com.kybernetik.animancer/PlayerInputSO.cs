using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "Scriptable Objects/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject
{
    pubic event UnityAction<Vector2> MoveEvent = delegate { };
}
