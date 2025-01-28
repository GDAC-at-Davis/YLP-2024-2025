using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "Scriptable Objects/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject
{
    Dictionary<int, PlayerInputEvents> _playerInputEvents = new();

    public UnityAction<Vector2> MoveEvent(int id) => _playerInputEvents[id].MoveEvent;
    public UnityAction<bool> LightAttackEvent(int id) => _playerInputEvents[id].LightAttackEvent;
    public UnityAction<bool> HeavyAttackEvent(int id) => _playerInputEvents[id].HeavyAttackEvent;
    public UnityAction<bool> SpecialAttackEvent(int id) => _playerInputEvents[id].SpecialAttackEvent;
    public UnityAction<bool> DashEvent(int id) => _playerInputEvents[id].DashEvent;
    public UnityAction<bool> JumpEvent(int id) => _playerInputEvents[id].JumpEvent;

    public class PlayerInputEvents
    {
        public UnityAction<Vector2> MoveEvent;
        public UnityAction<bool> LightAttackEvent;
        public UnityAction<bool> HeavyAttackEvent;
        public UnityAction<bool> SpecialAttackEvent;
        public UnityAction<bool> DashEvent;
        public UnityAction<bool> JumpEvent;
    }

    public void AddInputReader(int id)
    {
        _playerInputEvents.Add(id, new PlayerInputEvents());
    }
    public void RemoveInputReader(int id)
    {
        _playerInputEvents.Remove(id);
    }
    public PlayerInputEvents GetPlayerInputEvents(int id)
    {
        PlayerInputEvents inputEvents;
        return (!_playerInputEvents.TryGetValue(id, out inputEvents)) ? null : inputEvents;
    }
}
