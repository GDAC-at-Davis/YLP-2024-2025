using Managers;
using UnityEngine;

namespace Menus
{
    /// <summary>
    ///     Script that just clears all player data on start
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class ClearAllPlayerData : MonoBehaviour
    {
        [SerializeField]
        private GameDataSO gameDataSO;

        private void Awake()
        {
            gameDataSO.ClearPlayerData();
        }
    }
}