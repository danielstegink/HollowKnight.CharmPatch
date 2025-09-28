using CharmPatch.Charm_Patches.Helpers;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Mantis Arts increases the range of Nail Arts when Longnail and/or Mark of Pride are equipped
    /// </summary>
    public class MantisArts : Patch
    {
        public bool IsActive => SharedData.globalSettings.mantisArtsOn;

        public void Start()
        {
            On.HeroController.CharmUpdate += OnCharmUpdate;
        }

        /// <summary>
        /// This patch applies charm bonuses, so we can check if charms are equipped before triggering it
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void OnCharmUpdate(On.HeroController.orig_CharmUpdate orig, HeroController self)
        {
            orig(self);

            if (helper != null)
            {
                helper.Stop();
            }

            if (IsActive)
            {
                float modifier = GetModifier();
                if (modifier != 1f) // No need to strain the computer if there's no modifier to apply
                {
                    helper = new MantisArtsHelper(modifier);
                    helper.Start();
                }
            }
        }

        /// <summary>
        /// Utils helper
        /// </summary>
        MantisArtsHelper helper;

        /// <summary>
        /// Determines the appropriate modifier base on the equipped charms
        /// </summary>
        /// <returns></returns>
        private float GetModifier()
        {
            // Longnail increases nail range by 15%
            if (PlayerData.instance.GetBool("equippedCharm_18") &&
                !PlayerData.instance.GetBool("equippedCharm_13"))
            {
                if (SharedData.charmChangerMod != null &&
                    SharedData.globalSettings.charmChangerOn)
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
                if (SharedData.charmChangerMod != null &&
                    SharedData.globalSettings.charmChangerOn)
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
                if (SharedData.charmChangerMod != null &&
                    SharedData.globalSettings.charmChangerOn)
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