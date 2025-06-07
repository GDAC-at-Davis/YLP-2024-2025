using UnityEngine;

namespace Managers
{
    /// <summary>
    ///     Spawns global managers if they don't exist
    /// </summary>
    public class ManagerContainer : MonoBehaviour
    {
        [SerializeField]
        private GameObject managerPrefab;

        public static ManagerContainer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                if (managerPrefab != null)
                {
                    Instantiate(managerPrefab, transform);
                }
                else
                {
                    Debug.LogError("Manager prefab is not assigned in the ManagerContainer.");
                }

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}