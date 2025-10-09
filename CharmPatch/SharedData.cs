using CharmPatch.Charm_Patches;
using Modding;
using System.Collections.Generic;
namespace CharmPatch
{
    /// <summary>
    /// Stores variables and functions used by multiple files in this project
    /// </summary>
    public static class SharedData
    {
        /// <summary>
        /// Stores the global settings
        /// </summary>
        public static GlobalSettings globalSettings { get; set; } = new GlobalSettings();

        #region External Mods
        /// <summary>
        /// Unlimited Hiveblood
        /// </summary>
        public static IMod unlimitedHivebloodMod;

        /// <summary>
        /// Charm Changer
        /// </summary>
        public static IMod charmChangerMod;

        /// <summary>
        /// Exaltation
        /// </summary>
        public static IMod exaltationMod;

        /// <summary>
        /// Pale Court
        /// </summary>
        public static IMod paleCourtMod;

        /// <summary>
        /// Pale Court Charms
        /// </summary>
        public static IMod pcCharmsMod;

        /// <summary>
        /// Ancient Aspid
        /// </summary>
        public static IMod ancientAspidMod;
        #endregion

        /// <summary>
        /// List of currently supported charm patches
        /// </summary>
        public static List<Charm_Patches.Patch> charmPatches = new List<Charm_Patches.Patch>()
        {
            new BerserkersFury(),
            new DarkDashmaster(),
            new JonisKindness(),
            new OuterShell(),
            new MercifulMelody(),

            new GrubberflysReach(),
            new JonisElegy(),

            new CriticalBlow(),
            new MantisArts(),
            new MightyArts(),
            new QuickArts(),

            new BlueHive(),
            new MopRestored(),
            new FriendlyAspid(),
        };

        /// <summary>
        /// Stores certain properties for easy reference
        /// </summary>
        public static Dictionary<string, object> dataStore = new Dictionary<string, object>();
    }
}
