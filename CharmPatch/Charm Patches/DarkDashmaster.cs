using CharmPatch.OtherModHelpers;
using Newtonsoft.Json.Linq;

namespace CharmPatch.Charm_Patches
{
    public class DarkDashmaster : CharmPatch
    {
        /// <summary>
        /// Stores original Shadow Dash cooldown
        /// </summary>
        private float originalCooldown = -1f;

        /// <summary>
        /// Stores the Shadow Dash cooldown set by this patch
        /// </summary>
        private float moddedCooldown = -1f;

        public void AddHook()
        {
            On.HeroController.Update += Start;
        }

        /// <summary>
        /// Dark Dashmaster reduces the cooldown of Shadow Dash when Dashmaster is equipped
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void Start(On.HeroController.orig_Update orig, HeroController self)
        {
            //SharedData.Log("Dark Dashmaster check");

            // Buff once and only if Dashmaster is equipped and Dark Dashmaster is enabled
            // However, may need to update if the cooldown has been adjusted by another mod 
            if (SharedData.globalSettings.darkDashmasterOn &&
                PlayerData.instance.equippedCharm_31 &&
                moddedCooldown != self.SHADOW_DASH_COOLDOWN)
            {
                float modifier = GetModifier();
                originalCooldown = self.SHADOW_DASH_COOLDOWN;
                self.SHADOW_DASH_COOLDOWN *= modifier;
                moddedCooldown = self.SHADOW_DASH_COOLDOWN;
                //SharedData.Log($"Shadow dash cooldown: {originalCooldown} -> {self.SHADOW_DASH_COOLDOWN}");
            }

            // Make sure to reset if Dashmaster removed or patch disabled
            if ((!self.playerData.equippedCharm_31 || !SharedData.globalSettings.darkDashmasterOn) &&
                originalCooldown > 0)
            {
                self.SHADOW_DASH_COOLDOWN = originalCooldown;
                moddedCooldown = self.SHADOW_DASH_COOLDOWN;
                originalCooldown = -1;
                //SharedData.Log($"Shadow dash reset to {self.SHADOW_DASH_COOLDOWN}");
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