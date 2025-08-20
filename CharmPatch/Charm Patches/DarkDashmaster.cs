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
            if (IsActive)
            {
                On.HeroController.Start += StartDashHelper;

                if (HeroController.instance != null)
                {
                    helper = new DashHelper(1f, GetModifier());
                    helper.Start();
                }
            }
        }

        public void Stop()
        {
            On.HeroController.Start -= StartDashHelper;

            if (helper != null)
            {
                helper.Stop();
            }
        }

        /// <summary>
        /// DashHelper modifies the HeroController, so we should only initialize it when the HeroController has started
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void StartDashHelper(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);

            helper = new DashHelper(1f, GetModifier());
            helper.Start();
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
            if (SharedData.charmChangerMod != null)
            {
                float dashCooldown = (float)SharedData.dataStore["dash"];
                float dashmasterCooldown = (float)SharedData.dataStore["dashmaster"];
                return dashmasterCooldown / dashCooldown;
            }

            return 0.66f;
        }
    }
}