using Modding;
using System;

namespace CharmPatch.Charm_Patches
{
    public class DarkDashmaster : CharmPatch
    {
        public void AddHook()
        {
            On.HeroController.Update += Start;
        }

        /// <summary>
        /// Stores whether or not the patch has been applied
        /// </summary>
        private bool patchApplied = false;

        /// <summary>
        /// Dark Dashmaster reduces the cooldown of Shadow Dash when Dashmaster is equipped
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void Start(On.HeroController.orig_Update orig, HeroController self)
        {
            //SharedData.Log("Dark Dashmaster check");

            // Dashmaster reduces normal dash cooldown by 33%
            // NEW: Reduced by 40% to maintain base rate of 1 shadow dash per 3 regular dashes
            float modifier = 0.6f;

            // Buff once, and only if Dashmaster is equipped and Dark Dashmaster is enabled
            if (SharedData.globalSettings.darkDashmasterOn &&
                PlayerData.instance.equippedCharm_31 &&
                !patchApplied)
            {
                self.SHADOW_DASH_COOLDOWN *= modifier;
                //SharedData.Log($"Shadow dash cooldown set to {self.SHADOW_DASH_COOLDOWN}");
                patchApplied = true;
            }

            // Make sure to reset if Dashmaster removed or patch disabled
            if ((!self.playerData.equippedCharm_31 || !SharedData.globalSettings.darkDashmasterOn) &&
                patchApplied)
            {
                self.SHADOW_DASH_COOLDOWN /= modifier;
                //SharedData.Log($"Shadow dash reset to {self.SHADOW_DASH_COOLDOWN}");

                patchApplied = false;
            }

            orig(self);
        }
    }
}