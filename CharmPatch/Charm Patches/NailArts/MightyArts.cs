using System.Collections.Generic;

namespace CharmPatch.Charm_Patches
{
    internal class MightyArts : CharmPatch
    {
        public void AddHook()
        {
            On.HealthManager.TakeDamage += Start;
        }

        /// <summary>
        /// List of the object names of the Nail Art attacks
        /// </summary>
        private static List<string> nailArtNames = new List<string>()
        {
            "Cyclone Slash",
            "Great Slash",
            "Dash Slash",
            "Hit L",
            "Hit R"
        };

        /// <summary>
        /// Fragile/Unbreakable Strength increases the damage dealt by Nail Arts by 50%, just like regular nail attacks
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="hitInstance"></param>
        private void Start(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            if (PlayerData.instance.equippedCharm_25 &&
                SharedData.globalSettings.mightyArtsOn &&
                hitInstance.AttackType == AttackTypes.Nail)
            {
                string attackName = hitInstance.Source.name;
                if (nailArtNames.Contains(attackName))
                {
                    double bonusPercent = 0.50;
                    int bonusDamage = (int)(hitInstance.DamageDealt * bonusPercent);
                    hitInstance.DamageDealt += bonusDamage;
                }
            }

            orig(self, hitInstance);
        }
    }
}