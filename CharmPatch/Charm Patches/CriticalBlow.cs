using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    internal class CriticalBlow : CharmPatch
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
        /// Critical Blow increases the damage dealt by Nail Arts when Heavy Blow is equipped
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="hitInstance"></param>
        private void Start(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            //SharedData.Log($"Critical Blow Enabled: {SharedData.globalSettings.criticalBlowOn}");
            //SharedData.Log($"Heavy Blow Equipped: {PlayerData.instance.equippedCharm_15}");

            if (PlayerData.instance.equippedCharm_15 && 
                SharedData.globalSettings.criticalBlowOn &&
                hitInstance.AttackType == AttackTypes.Nail)
            {
                //SharedData.Log("Critical Blow enabled");

                string attackName = hitInstance.Source.name;
                //SharedData.Log($"Attack: {attackName}");

                if (nailArtNames.Contains(attackName))
                {
                    double bonusPercent = 0.30;
                    int bonusDamage = (int)(hitInstance.DamageDealt * bonusPercent);
                    //SharedData.Log($"{attackName}: {hitInstance.DamageDealt} damage increased by {bonusDamage}");

                    hitInstance.DamageDealt += bonusDamage;
                }
            }

            orig(self, hitInstance);
        }
    }
}