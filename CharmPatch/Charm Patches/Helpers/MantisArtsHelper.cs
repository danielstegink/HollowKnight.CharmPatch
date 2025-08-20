using CharmPatch.OtherModHelpers;
using DanielSteginkUtils.Components;
using DanielSteginkUtils.Helpers.Attributes;
using UnityEngine;

namespace CharmPatch.Charm_Patches.Helpers
{
    /// <summary>
    /// Helper class for Mantis Arts
    /// </summary>
    public class MantisArtsHelper : NailArtRangeHelper
    {
        public MantisArtsHelper(bool performLogging = false) :
            base(CharmPatch.Instance.Name, "Mantis Arts", 1f, performLogging) { }

        public override void ApplyBuff(GameObject gameObject, BuffNailArtSize modsApplied)
        {
            float customModifier = GetModifier();
            modsApplied.ModList[modName][featureName] = customModifier;
            if (performLogging)
            {
                CharmPatch.Instance.Log($"{featureName} - Size modifier {customModifier}");
            }

            base.ApplyBuff(gameObject, modsApplied);
        }

        private float GetModifier()
        {
            if (!SharedData.globalSettings.mantisArtsOn)
            {
                return 1f;
            }

            // Longnail increases nail range by 15%
            if (PlayerData.instance.GetBool("equippedCharm_18") &&
                !PlayerData.instance.GetBool("equippedCharm_13"))
            {
                if (SharedData.charmChangerMod != null)
                {
                    int scale = (int)SharedData.dataStore["longnail"];
                    return 1 + (float)scale / 100;
                }

                return 1.15f;
            }

            // Mark of Pride increases nail range by 25%
            if (!PlayerData.instance.GetBool("equippedCharm_18") &&
                PlayerData.instance.GetBool("equippedCharm_13"))
            {
                if (SharedData.charmChangerMod != null)
                {
                    int scale = (int)SharedData.dataStore["mop"];
                    return 1 + (float)scale / 100;
                }

                return 1.25f;
            }

            // The two stack on each other, so equipping both increases range by 40%
            if (PlayerData.instance.GetBool("equippedCharm_18") &&
                PlayerData.instance.GetBool("equippedCharm_13"))
            {
                if (SharedData.charmChangerMod != null)
                {
                    int scale = (int)SharedData.dataStore["lnMop"];
                    return 1 + (float)scale / 100;
                }

                return 1.4f;
            }

            return 1f;
        }
    }
}