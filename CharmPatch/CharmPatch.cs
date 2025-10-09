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

        public override string GetVersion() => "1.7.0.0";

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
            SharedData.paleCourtMod = ModHooks.GetMod("Pale Court");
            SharedData.pcCharmsMod = ModHooks.GetMod("PaleCourtCharms");
            SharedData.ancientAspidMod = ModHooks.GetMod("AncientAspid");

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
            //Log("Storing external data");
            SharedData.dataStore = new Dictionary<string, object>();

            if (SharedData.charmChangerMod != null)
            {
                SharedData.dataStore.Add("longnail", CharmChanger.GetProperty<int>("longnailScale"));
                SharedData.dataStore.Add("mop", CharmChanger.GetProperty<int>("markOfPrideScale"));
                SharedData.dataStore.Add("lnMop", CharmChanger.GetProperty<int>("longnailMarkOfPrideScale"));
                SharedData.dataStore.Add("strength", CharmChanger.GetProperty<int>("strengthDamageIncrease"));
                SharedData.dataStore.Add("hiveblood", CharmChanger.GetProperty<float>("hivebloodJonisTimer"));
                SharedData.dataStore.Add("dash", CharmChanger.GetProperty<float>("regularDashCooldown"));
                SharedData.dataStore.Add("dashmaster", CharmChanger.GetProperty<float>("dashmasterDashCooldown"));
                SharedData.dataStore.Add("nailCooldown", CharmChanger.GetProperty<float>("regularAttackCooldown"));
                SharedData.dataStore.Add("quickSlashCooldown", CharmChanger.GetProperty<float>("quickSlashAttackCooldown"));
            }

            if (SharedData.exaltationMod != null)
            {
                SharedData.dataStore.Add("stoneShellUnlocked", Exaltation.GetField<bool>("BaldurShellGlorified"));
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