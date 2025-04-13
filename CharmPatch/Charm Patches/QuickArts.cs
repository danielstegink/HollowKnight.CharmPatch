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

        private float modifier = 0.85f;

        private void Start(On.HeroController.orig_Update orig, HeroController self)
        {
            // if not updated and quick slash equipped, reduce nail charge times by 15%
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
