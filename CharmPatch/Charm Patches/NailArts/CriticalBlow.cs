using DanielSteginkUtils.Utilities;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Critical Blow increases the damage dealt by Nail Arts when Heavy Blow is equipped
    /// </summary>
    public class CriticalBlow : Patch
    {
        public bool IsActive => SharedData.globalSettings.criticalBlowOn;

        public void Start()
        {
            On.HealthManager.TakeDamage += BuffNailArts;
        }

        /// <summary>
        /// Increases the damage dealt by Nail Arts
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="hitInstance"></param>
        private void BuffNailArts(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            if (IsActive &&
                PlayerData.instance.GetBool("equippedCharm_15") && 
                Logic.IsNailArt(hitInstance.Source.name))
            {
                // Overall Heavy Blow feels like more of a 1-notch charm, so we should give 1 notch of extra Nail Art damage
                float modifier = 1 + NotchCosts.NailArtDamagePerNotch();
                int baseDamage = hitInstance.DamageDealt;
                hitInstance.DamageDealt = Calculations.GetModdedInt(baseDamage, modifier);
                //CharmPatch.Instance.Log($"Critical Blow - {baseDamage} damage increased by {modifier} to {hitInstance.DamageDealt}");
            }

            orig(self, hitInstance);
        }
    }
}