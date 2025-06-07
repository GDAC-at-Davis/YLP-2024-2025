using System;
using System.Collections.Generic;
using System.Linq;
using LevelScripts;
using NaughtyAttributes;
using Timeline.Samples;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Menus
{
    [CreateAssetMenu(fileName = "LevelSelectRoster", menuName = "Level Select Roster")]
    public class LevelSelectRoster : ScriptableObject
    {
        [Serializable]
        public struct LevelSelectData
        {
            public LevelSO Level;
            public bool IsHidden;
        }

        [NoFoldOut]
        public List<LevelSelectData> Levels;

#if UNITY_EDITOR
        /// <summary>
        ///     Search assets for LevelSOs and add them to the roster
        /// </summary>
        [Button("Autodetect Levels")]
        public void AutodetectLevels()
        {
            string[] assets = AssetDatabase.FindAssets("t:LevelSO");
            IEnumerable<LevelSO> levelSOs = assets.Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<LevelSO>);

            foreach (LevelSO foundLevel in levelSOs)
            {
                var found = false;
                foreach (LevelSelectData levelSelect in Levels)
                {
                    if (levelSelect.Level == foundLevel)
                    {
                        found = true;
                        break;
                    }
                }

                // Found a level not in the roster, add it in
                if (!found)
                {
                    Levels.Add(new LevelSelectData
                    {
                        Level = foundLevel,
                        IsHidden = false
                    });
                }
            }
        }
#endif
    }
}