using GameEntities;
using TMPro;
using UnityEngine;

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
    }

    void UpdateHealth(int health)
    {
        text.text = health.ToString();
    }
}
