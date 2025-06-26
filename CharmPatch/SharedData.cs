using CharmPatch.Charm_Patches;
using System.Collections.Generic;
using System.Reflection;
namespace CharmPatch
{
    /// <summary>
    /// Stores variables and functions used by multiple files in this project
    /// </summary>
    public static class SharedData
    {
        /// <summary>
        /// Used for sending messages to the ModLog
        /// </summary>
        private static CharmPatch _logger = new CharmPatch();

        /// <summary>
        /// Stores the global settings
        /// </summary>
        public static GlobalSettings globalSettings { get; set; } = new GlobalSettings();

        /// <summary>
        /// Tracks whether or not Charm Changer is installed
        /// </summary>
        public static bool charmChangerInstalled { get; set; } = false;

        /// <summary>
        /// Stores the numeric ID of the current save file
        /// </summary>
        public static int currentSave { get; set; } = -1;

        /// <summary>
        /// List of currently supported charm patches
        /// </summary>
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
        /// List of the object names of the Nail Art attacks
        /// </summary>
        public static List<string> nailArtNames = new List<string>()
        {
            "Cyclone Slash",
            "Great Slash",
            "Dash Slash",
            "Hit L",
            "Hit R"
        };

        /// <summary>
        /// Logs message to the shared mod log at AppData\LocalLow\Team Cherry\Hollow Knight\ModLog.txt
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            _logger.Log(message);
        }

        /// <summary>
        /// Gets a non-static field (even a private one) from the given input class
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static O GetField<I, O>(I input, string fieldName)
        {
            FieldInfo fieldInfo = input.GetType()
                                       .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (O)fieldInfo.GetValue(input);
        }

        /// <summary>
        /// Sets the value of non-static field (even a private one) in a given class
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetField<I>(I input, string fieldName, object value)
        {
            FieldInfo fieldInfo = input.GetType()
                                       .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(input, value);
        }
    }
}
