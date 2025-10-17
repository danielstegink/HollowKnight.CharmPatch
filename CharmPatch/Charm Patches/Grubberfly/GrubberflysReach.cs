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
            On.HeroController.CharmUpdate += OnCharmUpdate;
        }

        /// <summary>
        /// This patch starts a coroutine if the right charms are equipped, so we can check whenever charms are updated
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

            // If the patch is inactive or if it won't do anything, then there's no need to strain the machine
            if (IsActive &&
                (PlayerData.instance.GetBool("equippedCharm_18") ||
                    PlayerData.instance.GetBool("equippedCharm_13")))
            {
                helper = new GrubberflysReachHelper();
                helper.Start();
            }
        }

        /// <summary>
        /// Utils helper
        /// </summary>
        private GrubberflysReachHelper helper;
    }
}