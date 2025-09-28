using CharmPatch.Charm_Patches.Helpers;
using DanielSteginkUtils.Helpers.Abilities;
using System;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Dark Dashmaster reduces the cooldown of Shade Dash if Dashmaster is equipped
    /// </summary>
    public class DarkDashmaster : Patch
    {
        public bool IsActive => SharedData.globalSettings.darkDashmasterOn;

        public void Start()
        {
            On.HeroController.CharmUpdate += OnCharmUpdate;
        }

        /// <summary>
        /// Check each time charms update, since we only need to run if Dashmaster is equipped
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void OnCharmUpdate(On.HeroController.orig_CharmUpdate orig, HeroController self)
        {
            orig(self);

            if (helper != null)
            {
                helper.Stop();
            }

            if (IsActive &&
                PlayerData.instance.GetBool("equippedCharm_31"))
            {
                helper = new DashHelper(1f, GetModifier());
                helper.Start();
            }
        }

        /// <summary>
        /// Utils helper
        /// </summary>
        private DashHelper helper;

        /// <summary>
        /// By default, Dashmaster reduces the dash cooldown by 33%
        /// </summary>
        /// <returns></returns>
        private float GetModifier()
        {
            if (SharedData.charmChangerMod != null &&
                SharedData.globalSettings.charmChangerOn)
            {
                float dashCooldown = (float)SharedData.dataStore["dash"];
                float dashmasterCooldown = (float)SharedData.dataStore["dashmaster"];
                return dashmasterCooldown / dashCooldown;
            }

            return 0.66f;
        }
    }
}