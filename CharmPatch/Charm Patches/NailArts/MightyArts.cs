using CharmPatch.OtherModHelpers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CharmPatch.Charm_Patches
{
    public class MightyArts : CharmPatch
    {
        public void AddHook()
        {
            On.HealthManager.TakeDamage += Start;
        }

        /// <summary>
        /// Fragile/Unbreakable Strength increases the damage dealt by Nail Arts by 50%, just like regular nail attacks
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="hitInstance"></param>
        private void Start(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            //SharedData.Log("Damage Taken");
            //SharedData.Log($"Strength equipped: {PlayerData.instance.equippedCharm_25}");
            //SharedData.Log($"Mighty Arts on: {SharedData.globalSettings.mightyArtsOn}");
            //SharedData.Log($"Attack name: {hitInstance.Source.name}");

            if (PlayerData.instance.equippedCharm_25 &&
                SharedData.globalSettings.mightyArtsOn &&
                SharedData.nailArtNames.Contains(hitInstance.Source.name))
            {
                float bonusPercent = GetModifier();
                int bonusDamage = (int)(hitInstance.DamageDealt * bonusPercent);
                hitInstance.DamageDealt += bonusDamage;
            }

            orig(self, hitInstance);
        }

        /// <summary>
        /// Gets the damage modifier for Fragile/Unbreakable Strength
        /// </summary>
        /// <returns></returns>
        private float GetModifier()
        {
            // By default, Unbreakable Strength increases nail damage by 50%
            float modifier = 0.50f;

            if (SharedData.charmChangerInstalled)
            {
                JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "strengthDamageIncrease");
                modifier = float.Parse(modifierToken.ToString()) / 100;
            }

            //SharedData.Log($"Strength modifier: {modifier}");
            return modifier;
        }
    }
}