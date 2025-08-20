using CharmPatch.OtherModHelpers;
using DanielSteginkUtils.Components.Dung;
using DanielSteginkUtils.Helpers.Charms.Elegy;
using UnityEngine;

namespace CharmPatch.Charm_Patches.Helpers
{
    /// <summary>
    /// Helper class for Grubberfly's Reach
    /// </summary>
    public class GrubberflysReachHelper : ElegyBeamRangeHelper
    {
        public GrubberflysReachHelper(bool performLogging = false) :
            base(CharmPatch.Instance.Name, "Grubberfly's Elegy", 1f, performLogging) { }

        public override void ApplyBuff(GameObject gameObject, BuffElegyRange modsApplied)
        {
            string direction = GetDirection(gameObject.name);
            float customModifier = GetModifier(direction);
            modsApplied.ModList[modName][featureName] = customModifier;
            if (performLogging)
            {
                CharmPatch.Instance.Log($"Grubberfly's Reach - Range modifier {modsApplied.ModList[modName][featureName]}");
            }

            base.ApplyBuff(gameObject, modsApplied);
        }

        /// <summary>
        /// Gets how much to increase the beam's range by
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private float GetModifier(string direction)
        {
            // If Longnail is equipped, we want to increase the range by 15%
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

            // If Mark of Pride is equipped, we want to increase the range by 25%
            if (!PlayerData.instance.GetBool("equippedCharm_18") &&
                PlayerData.instance.GetBool("equippedCharm_13"))
            {
                // Unless we are swinging up, in which case the bonus is already applied
                if (direction.Equals("UP"))
                {
                    return 1f;
                }

                if (SharedData.charmChangerMod != null)
                {
                    int scale = (int)SharedData.dataStore["mop"];
                    return 1 + (float)scale / 100;
                }

                return 1.25f;
            }

            // Both are equipped, we combine them to increase the range by 40%
            if (PlayerData.instance.GetBool("equippedCharm_18") &&
                PlayerData.instance.GetBool("equippedCharm_13"))
            {
                // Again, unless we are already swinging up
                // In that case, we want to multiply the 25% already applied to reach 40%
                if (direction.Equals("UP"))
                {
                    if (SharedData.charmChangerMod != null)
                    {
                        int mopScale = (int)SharedData.dataStore["mop"];
                        int bothScale = (int)SharedData.dataStore["lnMop"];
                        float ratio = bothScale / mopScale;

                        return 1 + ratio / 100;
                    }

                    return 1.4f / 1.25f;
                }

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
