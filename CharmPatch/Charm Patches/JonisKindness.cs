namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Joni's Kindness increases the number of masks given by Joni's Blessing
    /// </summary>
    public class JonisKindness : Patch
    {
        public bool IsActive => SharedData.globalSettings.jonisKindnessOn;

        public void Start()
        {
            On.HeroController.CharmUpdate += BuffHealth;
        }

        /// <summary>
        /// The best time to adjust health is after sitting at a bench
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void BuffHealth(On.HeroController.orig_CharmUpdate orig, HeroController self)
        {
            orig(self);

            if (IsActive &&
                PlayerData.instance.GetBool("equippedCharm_27"))
            {
                PlayerData.instance.IntAdd("joniHealthBlue", 2);
                //CharmPatch.Instance.Log($"Joni's Kindness - Blue health increased to {PlayerData.instance.GetInt("joniHealthBlue")}");
            }
        }
    }
}