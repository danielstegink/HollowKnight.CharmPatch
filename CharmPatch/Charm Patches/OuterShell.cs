using Modding;

namespace CharmPatch.Charm_Patches
{
    internal class OuterShell : CharmPatch
    {
        public void AddHook()
        {
            ModHooks.TakeHealthHook += Start;
            On.PlayerData.MaxHealth += Refresh;
        }

        /// <summary>
        /// The number of extra hits Baldur Shell can take
        /// </summary>
        private readonly int extraHits = 2;

        // The number of extra hits Baldur Shell has taken since the last refresh
        private int extraHitsTaken = 0;

        /// <summary>
        /// Outer Shell lets Baldur Shell take 2 extra hits before breaking
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int Start(int damage)
        {
            SharedData.Log("Attacked");
            if (SharedData.globalSettings.outerShellOn &&
                PlayerData.instance.blockerHits < 4 && 
                extraHitsTaken < extraHits)
            {
                SharedData.Log("Outer shell hit");
                PlayerData.instance.blockerHits = 4;
                extraHitsTaken++;
            }

            return damage;
        }

        /// <summary>
        /// Resets the number of extra hits taken to 0
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void Refresh(On.PlayerData.orig_MaxHealth orig, PlayerData self)
        {
            extraHitsTaken = 0;

            orig(self);
        }
    }
}
