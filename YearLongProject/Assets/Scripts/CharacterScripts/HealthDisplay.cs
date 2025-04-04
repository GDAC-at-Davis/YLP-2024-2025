using GameEntities;
using TMPro;
using UnityEngine;

namespace CharacterScripts
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField]
        CharacterEntity entity;
        TextMeshProUGUI text;


        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();

            text.text = entity.Health.ToString();
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

        void UpdateHealth(int health)
        {
            text.text = health.ToString();
        }

        void UpdateInvincibilityStatus(bool isInvuln)
        {
            text.color = isInvuln ? Color.blue : Color.white;
        }
    }
}
