using DanielSteginkUtils.Helpers.Shields;
using DanielSteginkUtils.Utilities;
using System.Collections;
using UnityEngine;

namespace CharmPatch.Charm_Patches.Helpers
{
    /// <summary>
    /// Helper for Berserker's Fury
    /// </summary>
    public class FuryShield : ShieldHelper
    {
        /// <summary>
        /// Give a random chance of triggering, but only if FOTF is active
        /// </summary>
        /// <returns></returns>
        public override bool CustomShieldCheck()
        {
            // Confirm Fury is active 
            GameObject charmEffectsObject = HeroController.instance.gameObject.transform.Find("Charm Effects").gameObject;
            PlayMakerFSM fury = charmEffectsObject.LocateMyFSM("Fury");
            //CharmPatch.Instance.Log($"Fury Shield - Fury state {fury.ActiveStateName}");
            if (fury.ActiveStateName.Equals("Activate") ||
                fury.ActiveStateName.Equals("Stay Furied"))
            {
                // FOTF is almost worth its 2 notches, so this shield should probly only
                // be worth about 1/2 a notch, or a 3% chance
                int random = UnityEngine.Random.Range(1, 101);
                int blockChance = (int)(NotchCosts.ShieldChancePerNotch() / 2);
                //CharmPatch.Instance.Log($"Fury shield - {random} vs {blockChance}");
                if (random <= blockChance)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Since this shield is from FOTF, it makes sense for it to flash red when triggered
        /// </summary>
        /// <returns></returns>
        public override IEnumerator CustomEffects()
        {
            HeroController.instance.GetComponent<SpriteFlash>().flash(Color.red, 0.7f, 0.4f, 0.5f, 0.4f);
            return base.CustomEffects();
        }
    }
}
