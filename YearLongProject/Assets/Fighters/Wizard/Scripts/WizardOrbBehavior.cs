using GameEntities;
using Hitbox.DataStructures;
using Hitbox.System;
using UnityEngine;

namespace Fighters.Wizard.Scripts
{
    public class WizardOrbBehavior : Entity
    {
        WizardOrbManager manager;
        int orbID;
        public int OrbID => orbID;

        public void Initialize(WizardOrbManager manager)
        {
            this.manager = manager;
            gameObject.SetActive(false);
        }

        public override void OnHitByAttack(HitboxInstance hitboxInstance, HitImpact hitImpact)
        {
            if (hitboxInstance.Context.Source != manager.Wizard)
            {
                return;
            }

            Debug.Log("owie :[");
        }
    }
}
