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

        public static List<Charm_Patches.CharmPatch> charmPatches = new List<Charm_Patches.CharmPatch>()
        {
            new BerserkersFury(),
            new BlueHive(),
            new CriticalBlow(),
            new DarkDashmaster(),
            new JonisKindness(),
            new MantisArts(),
            new MercifulMelody(),
            new QuickArts(),
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
