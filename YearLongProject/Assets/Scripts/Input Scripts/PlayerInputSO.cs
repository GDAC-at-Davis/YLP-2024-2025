using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "Scriptable Objects/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject
{
    public class PlayerInputEvents
    {
        public UnityAction<Vector2> MoveEvent;
        public UnityAction<bool> LightAttackEvent;
        public UnityAction<bool> HeavyAttackEvent;
        public UnityAction<bool> SpecialAttackEvent;
        public UnityAction<bool> DashEvent;
        public UnityAction<bool> JumpEvent;
    }

    private readonly Dictionary<int, PlayerInputEvents> _playerInputEvents = new();

    public ref UnityAction<Vector2> MoveEvent(int id)
    {
        return ref TryGetInputEvents(id).MoveEvent;
    }

    public ref UnityAction<bool> LightAttackEvent(int id)
    {
        return ref TryGetInputEvents(id).LightAttackEvent;
    }

    public UnityAction<bool> HeavyAttackEvent(int id)
    {
        return TryGetInputEvents(id).HeavyAttackEvent;
    }

    public UnityAction<bool> SpecialAttackEvent(int id)
    {
        return TryGetInputEvents(id).SpecialAttackEvent;
    }

    public UnityAction<bool> DashEvent(int id)
    {
        return TryGetInputEvents(id).DashEvent;
    }

    public UnityAction<bool> JumpEvent(int id)
    {
        return TryGetInputEvents(id).JumpEvent;
    }

    public PlayerInputEvents TryGetInputEvents(int id)
    {
        if (!_playerInputEvents.TryGetValue(id, out _))
        {
            _playerInputEvents.Add(id, new PlayerInputEvents());
        }

        return _playerInputEvents[id];
    }

    public void RemoveInputReader(int id)
    {
        if (!_playerInputEvents.ContainsKey(id))
        {
            return;
        }

        _playerInputEvents.Remove(id);
    }

    public PlayerInputEvents GetPlayerInputEvents(int id)
    {
        PlayerInputEvents inputEvents;
        return !_playerInputEvents.TryGetValue(id, out inputEvents) ? null : inputEvents;
    }
}