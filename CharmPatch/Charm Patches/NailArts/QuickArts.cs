namespace CharmPatch.Charm_Patches
{
    public class QuickArts : CharmPatch
    {
        /// <summary>
        /// Tracks whether we've applied the buff or not
        /// </summary>
        private bool updated = false;

        // Stores the Quick Slash modifier
        private float modifier = 1.0f;

        public void AddHook()
        {
            On.HeroController.CharmUpdate += Start;
        }

        /// <summary>
        /// Quick Arts makes Quick Slash reduce the cooldown of nail arts
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void Start(On.HeroController.orig_CharmUpdate orig, HeroController self)
        {
            if (SharedData.globalSettings.quickArtsOn && 
                PlayerData.instance.equippedCharm_32 &&
                !updated)
            {
                modifier = GetModifier(self);
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

            // Manually set the charge time
            //SharedData.Log($"Quick Arts: original charge time: {SharedData.GetField<HeroController, float>(self, "nailChargeTime")}");
            if (PlayerData.instance.equippedCharm_26)
            {
                SharedData.SetField(self, "nailChargeTime", self.NAIL_CHARGE_TIME_CHARM);
            }
            else
            {
                SharedData.SetField(self, "nailChargeTime", self.NAIL_CHARGE_TIME_DEFAULT);
            }
            //SharedData.Log($"Quick Arts: final charge time: {SharedData.GetField<HeroController, float>(self, "nailChargeTime")}");
        }

        /// <summary>
        /// Gets the cooldown modifier of the Quick Slash charm
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private float GetModifier(HeroController self)
        {
            // By default, Quick Slash reduced cooldown time by 39%
            float modifier = 0.61f;

            // If Charm Changer is installed, get the cooldown times 
            // it sets to determine that new Quick Slash modifier
            if (SharedData.charmChangerInstalled)
            {
                float normalCooldown = self.ATTACK_COOLDOWN_TIME;
                float charmCooldown = self.ATTACK_COOLDOWN_TIME_CH;
                //SharedData.Log($"Normal attack: {normalCooldown}, Quick attack: {charmCooldown}");

                modifier = charmCooldown / normalCooldown;
            }

            //SharedData.Log($"Quick Slash modifier: {modifier}");
            return modifier;
        }
    }
}
