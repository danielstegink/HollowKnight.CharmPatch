using CharmPatch.OtherModHelpers;
using Newtonsoft.Json.Linq;

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
        /// Stores the dash modifier of Dashmaster
        /// </summary>
        private float modifier = 1f;

        /// <summary>
        /// Dark Dashmaster reduces the cooldown of Shadow Dash when Dashmaster is equipped
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void Start(On.HeroController.orig_Update orig, HeroController self)
        {
            //SharedData.Log("Dark Dashmaster check");

            // Buff once, and only if Dashmaster is equipped and Dark Dashmaster is enabled
            if (SharedData.globalSettings.darkDashmasterOn &&
                PlayerData.instance.equippedCharm_31 &&
                !patchApplied)
            {
                modifier = GetModifier();
                float originalCooldown = self.SHADOW_DASH_COOLDOWN;
                self.SHADOW_DASH_COOLDOWN *= modifier;
                //SharedData.Log($"Shadow dash cooldown: {originalCooldown} -> {self.SHADOW_DASH_COOLDOWN}");
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

        /// <summary>
        /// Calculates the desired cooldown modifier for Dashmaster
        /// </summary>
        /// <returns></returns>
        private float GetModifier()
        {
            // By default, Dashmaster reduces the dash cooldown by 33%
            float modifier = 0.66f;

            if (SharedData.charmChangerInstalled)
            {
                JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "regularDashCooldown");
                float regularDash = float.Parse(modifierToken.ToString());

                modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "dashmasterDashCooldown");
                float darkDash = float.Parse(modifierToken.ToString());

                modifier = darkDash / regularDash;
            }

            //SharedData.Log($"Dashmaster modifier: {modifier}");
            return modifier;
        }
    }
}