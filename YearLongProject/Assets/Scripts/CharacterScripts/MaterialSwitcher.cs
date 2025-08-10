using System.Collections.Generic;
using EditorUtils.BoldHeader;
using GameEntities;
using NaughtyAttributes;
using UnityEngine;

namespace CharacterScripts
{
    public class MaterialSwitcher : MonoBehaviour
    {
        [BoldHeader("Material Switcher")]
        [InfoBox("Switches material based on player number")]
        [SerializeField]
        private List<Material> materials;

        [SerializeField]
        private Renderer renderer;

        [Tooltip("Optional - Particle System to assign material to")]
        [SerializeField]
        private ParticleSystemRenderer particleSystemRenderer;

        [SerializeField]
        private CharacterEntity characterEntity;

        private void Start()
        {
            int playerId = characterEntity.PlayerId;

            if (playerId < 0 || playerId >= materials.Count)
            {
                Debug.LogError($"Player ID {playerId} is out of range for materials list.", this);
                return;
            }

            renderer.material = materials[playerId];
            if (particleSystemRenderer != null)
            {
                particleSystemRenderer.material = materials[playerId];
            }
        }
    }
}