using DanielSteginkUtils.Utilities;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Mighty Arts increases the damage dealt by Nail Arts when Fragile/Unbreakable Strength is equipped
    /// </summary>
    public class MightyArts : Patch
    {
        public bool IsActive => SharedData.globalSettings.mightyArtsOn;

        public void Start()
        {
            On.HealthManager.TakeDamage += BuffNailArts;
        }

        /// <summary>
        /// Increases damage dealt by nail attacks
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="hitInstance"></param>
        private void BuffNailArts(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            if (IsActive &&
                PlayerData.instance.GetBool("equippedCharm_25") &&
                Logic.IsNailArt(hitInstance.Source.name))
            {
                float modifier = GetModifier();
                int baseDamage = hitInstance.DamageDealt;
                hitInstance.DamageDealt = Calculations.GetModdedInt(baseDamage, modifier);
                //CharmPatch.Instance.Log($"Mighty Arts - {baseDamage} damage increased by {modifier} to {hitInstance.DamageDealt}");
            }

            orig(self, hitInstance);
        }

        /// <summary>
        /// Fragile/Unbreakable Strength increases the damage dealt by Nail Arts by 50%, just like regular nail attacks
        /// </summary>
        /// <returns></returns>
        private float GetModifier()
        {
            if (SharedData.charmChangerMod != null &&
                SharedData.globalSettings.charmChangerOn)
            {
                int scale = (int)SharedData.dataStore["strength"];
                return 1 + (float)scale / 100;
            }

            return 1.5f;
        }
    }
}