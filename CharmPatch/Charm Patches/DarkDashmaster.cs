using Modding;

namespace CharmPatch.Charm_Patches
{
    public class DarkDashmaster : CharmPatch
    {
        public void AddHook()
        {
            ModHooks.CharmUpdateHook += Start;
        }

        /// <summary>
        /// Stores the original cooldown
        /// </summary>
        private float baseCooldown = 0f;

        /// <summary>
        /// Reduces the cooldown of Shadow dash when Dashmaster is equipped
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        private void Start(PlayerData data, HeroController controller)
        {
            // Buff once, and only if Dashmaster is equipped
            if (SharedData.globalSettings.darkDashmasterOn &&
                data.equippedCharm_31 && 
                baseCooldown == 0)
            {
                baseCooldown = controller.SHADOW_DASH_COOLDOWN;

                // Dashmaster reduces normal dash cooldown by 33%
                // NEW: Reduced by 40% to maintain base rate of 1 shadow dash per 3 regular dashes
                float newCooldown = baseCooldown * 0.6f;
                controller.SHADOW_DASH_COOLDOWN = newCooldown;
                SharedData.Log($"Shadow dash: {baseCooldown} -> {newCooldown}");
            }

            // Make sure to reset if Dashmaster removed or patch disabled
            if ((!data.equippedCharm_31 || !SharedData.globalSettings.darkDashmasterOn) && 
                baseCooldown != 0)
            {
                controller.SHADOW_DASH_COOLDOWN = baseCooldown;
                SharedData.Log($"Shadow dash reset to {baseCooldown}");

                baseCooldown = 0f;
            }
        }
    }
}