using CharmPatch.OtherModHelpers;
using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharmPatch
{
    public class CharmPatch : Mod, ICustomMenuMod, IGlobalSettings<GlobalSettings>
    {
        public static CharmPatch Instance;

        public override string GetVersion() => "1.5.1.0";

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
            Log("Initializing");

            Instance = this;

            // Get the external mods
            SharedData.unlimitedHivebloodMod = ModHooks.GetMod("UnlimitedHiveblood");
            SharedData.exaltationMod = ModHooks.GetMod("Exaltation");
            SharedData.charmChangerMod = ModHooks.GetMod("CharmChanger");

            // Start the patches
            StorePatchData();
            foreach (Charm_Patches.Patch patch in SharedData.charmPatches)
            {
                patch.Start();
            }

            ModHooks.SavegameSaveHook += OnSave;

            Log("Initialized");
        }

        /// <summary>
        /// Gets data related to patches, such as Charm Changer settings, and stores
        /// it for ease of reference
        /// </summary>
        private void StorePatchData()
        {
            Log("Storing external data");
            if (SharedData.charmChangerMod != null)
            {
                Dictionary<string, object> cache = new Dictionary<string, object>
                {
                    { "longnail", CharmChanger.GetProperty<int>("longnailScale") },
                    { "mop", CharmChanger.GetProperty<int>("markOfPrideScale") },
                    { "lnMop", CharmChanger.GetProperty<int>("longnailMarkOfPrideScale") },
                    { "strength", CharmChanger.GetProperty<int>("strengthDamageIncrease") },
                    { "hiveblood", CharmChanger.GetProperty<float>("hivebloodJonisTimer") },
                    { "dash", CharmChanger.GetProperty<float>("regularDashCooldown") },
                    { "dashmaster", CharmChanger.GetProperty<float>("dashmasterDashCooldown") },
                    { "stoneShellUnlocked", Exaltation.GetField<bool>("BaldurShellGlorified") },
                    { "nailCooldown", CharmChanger.GetProperty<float>("regularAttackCooldown") },
                    { "quickSlashCooldown", CharmChanger.GetProperty<float>("quickSlashAttackCooldown") },
                };

                SharedData.dataStore = cache;
            }
        }

        /// <summary>
        /// We will reset the patches when the player saves
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnSave(int obj)
        {
            StorePatchData();

            Log("Resetting patches");
            foreach (Charm_Patches.Patch patch in SharedData.charmPatches)
            {
                patch.Stop();
                patch.Start();
            }
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