using System;
using UnityEngine;

namespace Events
{
    public abstract class SoEvent : ScriptableObject
    {
        [SerializeField]
        [TextArea]
        private string description;

        protected Action InternalVoidEvent;

        public void AddListener(Action listener)
        {
            InternalVoidEvent += listener;
        }

        public void RemoveListener(Action listener)
        {
            InternalVoidEvent -= listener;
        }
    }

    public abstract class SoEvent<T> : SoEvent
    {
        private event Action<T> InternalEvent;

        public void Raise(T value)
        {
#if UNITY_EDITOR
            Debug.Log($"Event {name} raised with value {value}");
#endif
            InternalEvent?.Invoke(value);
            InternalVoidEvent?.Invoke();
        }

        public void AddListener(Action<T> listener)
        {
            InternalEvent += listener;
        }

        public void RemoveListener(Action<T> listener)
        {
            InternalEvent -= listener;
        }
    }
}