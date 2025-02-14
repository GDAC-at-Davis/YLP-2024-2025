using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Input_Scripts
{
    /// <summary>
    ///     Usable input service that Characters interface with
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "Scriptable Objects/PlayerInputSO")]
    public class PlayerInputSo : ScriptableObject
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

        public UnityAction<int> PlayerInputAdded;

        private readonly Dictionary<int, PlayerInputEvents> playerInputEvents = new();

        public ref UnityAction<Vector2> MoveEvent(int id)
        {
            return ref TryGetPlayerInputEvents(id).MoveEvent;
        }

        public ref UnityAction<bool> LightAttackEvent(int id)
        {
            return ref TryGetPlayerInputEvents(id).LightAttackEvent;
        }

        public UnityAction<bool> HeavyAttackEvent(int id)
        {
            return TryGetPlayerInputEvents(id).HeavyAttackEvent;
        }

        public UnityAction<bool> SpecialAttackEvent(int id)
        {
            return TryGetPlayerInputEvents(id).SpecialAttackEvent;
        }

        public UnityAction<bool> DashEvent(int id)
        {
            return TryGetPlayerInputEvents(id).DashEvent;
        }

        public UnityAction<bool> JumpEvent(int id)
        {
            return TryGetPlayerInputEvents(id).JumpEvent;
        }

        public PlayerInputEvents TryGetPlayerInputEvents(int id)
        {
            if (!playerInputEvents.TryGetValue(id, out _))
            {
                playerInputEvents.Add(id, new PlayerInputEvents());
                PlayerInputAdded?.Invoke(id);
            }

            return playerInputEvents[id];
        }

        public void RemoveInputReader(int id)
        {
            if (!playerInputEvents.ContainsKey(id))
            {
                return;
            }

            playerInputEvents.Remove(id);
        }
    }
}