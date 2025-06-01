using CharmPatch.Charm_Patches;
using System.Collections.Generic;
namespace CharmPatch
{
    /// <summary>
    /// Stores variables and functions used by multiple files in this project
    /// </summary>
    public static class SharedData
    {
        private static CharmPatch _logger = new CharmPatch();

        public static GlobalSettings globalSettings { get; set; } = new GlobalSettings();

        public static bool charmChangerInstalled { get; set; } = false;

        public static int currentSave { get; set; } = -1;

        public static List<Charm_Patches.CharmPatch> charmPatches = new List<Charm_Patches.CharmPatch>()
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
        };

        /// <summary>
        /// Logs message to the shared mod log at AppData\LocalLow\Team Cherry\Hollow Knight\ModLog.txt
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            _logger.Log(message);
        }
    }
}
