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
            if (IsActive)
            {
                helper = new FuryShield();
                helper.Start();
            }
        }

        public void Stop()
        {
            if (helper != null)
            {
                helper.Stop();
            }
        }

        /// <summary>
        /// Utils helper
        /// </summary>
        FuryShield helper;
    }
}