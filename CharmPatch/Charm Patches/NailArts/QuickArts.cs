using HKMirror.Reflection.SingletonClasses;
using System;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Quick Arts reduces the charge time of Nail Arts when Quick Slash is equipped
    /// </summary>
    public class QuickArts : Patch
    {
        public bool IsActive => SharedData.globalSettings.quickArtsOn;

        public void Start()
        {
            On.HeroController.CharmUpdate += BuffNailArts;
        }

        /// <summary>
        /// The Nail Charge animation is handled by the HC in the Update method, but the actual time is set by CharmUpdate
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void BuffNailArts(On.HeroController.orig_CharmUpdate orig, HeroController self)
        {
            orig(self);

            // Only modify the charge time if Quick Slash is equipped
            if (IsActive && 
                PlayerData.instance.GetBool("equippedCharm_32"))
            {
                float modifier = GetModifier(self);
                HeroControllerR.nailChargeTime *= modifier;
                //CharmPatch.Instance.Log($"Quick Arts - Required time reduced by {modifier} to {HeroControllerR.nailChargeTime}");
            }
        }

        /// <summary>
        /// By default, Quick Slash reduces the cooldown time by 39%
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private float GetModifier(HeroController self)
        {
            // If Charm Changer is installed, get the cooldown times it sets to determine the new Quick Slash modifier
            if (SharedData.charmChangerMod != null &&
                SharedData.globalSettings.charmChangerOn)
            {
                float normalCooldown = (float)SharedData.dataStore["nailCooldown"];
                float charmCooldown = (float)SharedData.dataStore["quickSlashCooldown"];
                //CharmPatch.Instance.Log($"Quick Arts - {charmCooldown} / {normalCooldown} = {charmCooldown / normalCooldown}");
                return charmCooldown / normalCooldown;
            }

            return 0.61f;
        }
    }
}