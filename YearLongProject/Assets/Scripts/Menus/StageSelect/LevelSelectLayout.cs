using UnityEngine;
using UnityEngine.UI;

namespace Menus.StageSelect
{
    /// <summary>
    ///     populates level select menu with levels based on levelSO's in levellist
    /// </summary>
    public class LevelSelectLayout : MonoBehaviour
    {
        [SerializeField]
        private LevelSelectRoster levelList;

        [SerializeField]
        private LevelSelectButton buttonTemplate;

        private void Start()
        {
            var gridLayout = GetComponent<GridLayoutGroup>();
            foreach (LevelSelectRoster.LevelSelectData level in levelList.Levels)
            {
                if (level.IsHidden)
                {
                    continue;
                }

                Instantiate(buttonTemplate, gridLayout.transform).Init(level.Level);
            }
        }
    }
}