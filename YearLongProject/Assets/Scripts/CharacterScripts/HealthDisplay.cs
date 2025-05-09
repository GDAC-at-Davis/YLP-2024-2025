using GameEntities;
using Managers;
using TMPro;
using UnityEngine;

namespace CharacterScripts
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField]
        CharacterEntity entity;
        TextMeshProUGUI text;
        [SerializeField]
        GameDataSO gameDataSO;

        Color baseColor;


        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();

            text.text = entity.Health.ToString();
            text.color = baseColor = gameDataSO.PlayerColors[entity.PlayerId];
        }

        private void OnEnable()
        {
            entity.UpdateHealth += UpdateHealth;
            entity.InvincibleChanged += UpdateInvincibilityStatus;
        }

        private void OnDisable()
        {
            entity.UpdateHealth -= UpdateHealth;
            entity.InvincibleChanged -= UpdateInvincibilityStatus;
        }

        void UpdateHealth(int id, int health)
        {
            text.text = health.ToString();
        }

        void UpdateInvincibilityStatus(bool isInvuln)
        {
            text.color = isInvuln ? Color.white : baseColor;
        }
    }
}
