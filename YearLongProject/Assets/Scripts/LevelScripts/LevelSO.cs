using Base;
using NaughtyAttributes;
using UnityEngine;

namespace LevelScripts
{
    /// <summary>
    /// Scriptable Object that holds relevant level information for reference
    /// </summary>
    [CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        [Tooltip("Character Name")]
        public string LevelDisplayName;

        [Tooltip("Reference for level's gameobject")]
        public GameObject LevelPrefab;

        [ShowAssetPreview(128, 128)]
        [Tooltip("Level portrait for use in UI Elements")]
        public Sprite LevelPortrait;
    }
}
