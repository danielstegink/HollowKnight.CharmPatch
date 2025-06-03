using CharmPatch.Charm_Patches;
using Modding;
using System.Collections.Generic;
using UnityEngine;

namespace CharmPatch
{
    public class CharmPatch : Mod, ICustomMenuMod, IGlobalSettings<GlobalSettings>
    {
        public override string GetVersion() => "1.4.1.0";

        public void OnLoadGlobal(GlobalSettings s)
        {
            SharedData.globalSettings = s;
        }

        public GlobalSettings OnSaveGlobal()
        {
            return SharedData.globalSettings;
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            //SharedData.Log("Starting mod");

            foreach (Charm_Patches.CharmPatch patch in SharedData.charmPatches)
            {
                patch.AddHook();
            }

            // Check if Unlimited Hiveblood is installed so Blue Hive can decide whether to work or not
            Charm_Patches.CharmPatch blueHive = SharedData.charmPatches[11];
            if (blueHive is BlueHive)
            {
                ((BlueHive)blueHive).unlimitedHiveblood = ModHooks.GetMod("UnlimitedHiveblood");
            }

            // Note if Charm Changer is installed
            // If so, we will need to grab its settings
            IMod charmChanger = ModHooks.GetMod("CharmChanger");
            if (charmChanger != null)
            {
                SharedData.charmChangerInstalled = true;
            }
            //SharedData.Log($"Charm Changer installed: {SharedData.charmChangerInstalled}");

            ModHooks.SavegameLoadHook += NewGame;

            //SharedData.Log("Startup complete");
        }

        /// <summary>
        /// When a new game is loaded, we need to store the 
        /// save file's index, in case we need to reference
        /// data in the save file
        /// </summary>
        /// <param name="saveIndex"></param>
        private void NewGame(int saveIndex)
        {
            SharedData.currentSave = saveIndex;
        }

        #region Menu Options
        public bool ToggleButtonInsideMenu => false;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            //SharedData.Log("Getting menu");
            return ModMenu.CreateMenuScreen(modListMenu);
        }
        #endregion
    }
}