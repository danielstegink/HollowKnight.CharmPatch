using Modding;
using System.Collections.Generic;
using UnityEngine;

namespace CharmPatch
{
    public class CharmPatch : Mod, IMenuMod, IGlobalSettings<GlobalSettings>
    {
        public override string GetVersion() => "1.2.0.1";

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

            //SharedData.Log("Startup complete");
        }

        #region Menu Options
        public bool ToggleButtonInsideMenu => false;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            SharedData.Log("Getting menu");
            return ModMenu.CreateMenu();
        }
        #endregion
    }
}