using CharmPatch.Charm_Patches.Helpers;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Grubberfly's Reach increases the range of Grubberfly's Elegy is Longnail and/or Mark of Pride are equipped
    /// </summary>
    public class GrubberflysReach : Patch
    {
        public bool IsActive => SharedData.globalSettings.grubberflysReachOn;

        public void Start()
        {
            if (IsActive)
            {
                helper = new GrubberflysReachHelper();
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

        private GrubberflysReachHelper helper;

    }
}