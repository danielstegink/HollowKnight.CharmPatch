using CharmPatch.Charm_Patches;
using Modding;
using System.Collections.Generic;
using UnityEngine;

namespace CharmPatch
{
    public class CharmPatch : Mod, ICustomMenuMod, IGlobalSettings<GlobalSettings>
    {
        public override string GetVersion() => "1.3.0.0";

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

            Charm_Patches.CharmPatch blueHive = SharedData.charmPatches[11];
            if (blueHive is BlueHive)
            {
                ((BlueHive)blueHive).unlimitedHiveblood = ModHooks.GetMod("UnlimitedHiveblood");
            }

            //SharedData.Log("Startup complete");
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