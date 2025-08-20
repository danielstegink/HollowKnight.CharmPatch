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
            if (IsActive)
            {
                helper = new MantisArtsHelper();
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
        MantisArtsHelper helper;
    }
}