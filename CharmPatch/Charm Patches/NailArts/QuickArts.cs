namespace CharmPatch.Charm_Patches
{
    public class QuickArts : CharmPatch
    {
        public void AddHook()
        {
            On.HeroController.Update += Start;
        }

        /// <summary>
        /// Tracks whether we've applied the buff or not
        /// </summary>
        private bool updated = false;

        /// <summary>
        /// Quick Slash reduces the cooldown of nail strikes by 39%
        /// </summary>
        private float modifier = 0.61f;

        /// <summary>
        /// Quick Arts makes Quick Slash reduce the cooldown of nail arts
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void Start(On.HeroController.orig_Update orig, HeroController self)
        {
            if (SharedData.globalSettings.quickArtsOn && 
                PlayerData.instance.equippedCharm_32 &&
                !updated)
            {
                self.NAIL_CHARGE_TIME_CHARM *= modifier;
                self.NAIL_CHARGE_TIME_DEFAULT *= modifier;

                updated = true;
                //SharedData.Log($"Quick Arts: default charge time set to {self.NAIL_CHARGE_TIME_DEFAULT}, nmg time set to {self.NAIL_CHARGE_TIME_CHARM}");
            }
            else if ((!SharedData.globalSettings.quickArtsOn || !PlayerData.instance.equippedCharm_32) &&
                updated) // If updated and no longer enabled, reset
            {
                self.NAIL_CHARGE_TIME_CHARM /= modifier;
                self.NAIL_CHARGE_TIME_DEFAULT /= modifier;

                updated = false;
                //SharedData.Log($"Quick Arts: default charge time reset to {self.NAIL_CHARGE_TIME_DEFAULT}, nmg time reset to {self.NAIL_CHARGE_TIME_CHARM}");
            }

            orig(self);
        }
    }
}
