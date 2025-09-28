using CharmPatch.Charm_Patches.Helpers;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Berserker's Fury adds a chance for FOTF to negate damage when active
    /// </summary>
    public class BerserkersFury : Patch
    {
        public bool IsActive => SharedData.globalSettings.berserkersFuryOn;

        public void Start()
        {
            On.HeroController.CharmUpdate += OnCharmUpdate;
        }

        /// <summary>
        /// We want to reset whenever charms are updated, since we only want to run if FOTF is equipped
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

            if (IsActive &&
                PlayerData.instance.GetBool("equippedCharm_6"))
            {
                helper = new FuryShield();
                helper.Start();
            }
        }

        /// <summary>
        /// Utils helper
        /// </summary>
        FuryShield helper;
    }
}