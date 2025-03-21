using Satchel.BetterMenus;
using System;

namespace CharmPatch
{
    public static class ModMenu
    {
        public static MenuScreen CreateMenu(MenuScreen modListMenu)
        {
            Element[] menuOptions = new Element[]
            {
                new BoolSetting("Dark Dashmaster", "Cooldown of Shadow Dash reduced when Dashmaster equipped",
                    newValue =>
                    {
                        SharedData.globalSettings.darkDashmasterEnabled = newValue;
                    },
                    () => SharedData.globalSettings.darkDashmasterEnabled,
                    "darkDashmaster"),
                new BoolSetting("Cheaper Dreamshield", "Cost of Dreamshield reduced by 1",
                    newValue =>
                    {
                        SharedData.globalSettings.cheaperDreamshieldEnabled = newValue;
                    },
                    () => SharedData.globalSettings.cheaperDreamshieldEnabled,
                    "cheaperDreamshield"),
                new BoolSetting("Ferocious Grimmchild", "Grimmchild's attack speed increased",
                    newValue =>
                    {
                        SharedData.globalSettings.ferociousGrimmchildEnabled = newValue;
                    },
                    () => SharedData.globalSettings.ferociousGrimmchildEnabled,
                    "ferociousGrimmchild"),
                new BoolSetting("Berserker's Fury", "Chance for Fury of the Fallen to ignore damage while active",
                    newValue =>
                    {
                        SharedData.globalSettings.berserkersFuryEnabled = newValue;
                    },
                    () => SharedData.globalSettings.berserkersFuryEnabled,
                    "berserkersFury"),
                new BoolSetting("Merciful Melody", "Chance to heal when Carefree Melody is triggered",
                    newValue =>
                    {
                        SharedData.globalSettings.mercifulMelodyEnabled = newValue;
                    },
                    () => SharedData.globalSettings.mercifulMelodyEnabled,
                    "mercifulMelody"),
                new BoolSetting("Outer Shell", "Baldur Shell can block more times",
                    newValue =>
                    {
                        SharedData.globalSettings.outerShellEnabled = newValue;
                    },
                    () => SharedData.globalSettings.outerShellEnabled,
                    "outerShell"),
                new BoolSetting("Joni's Kindness", "Joni's Blessing gives more Lifeblood Masks",
                    newValue =>
                    {
                        SharedData.globalSettings.jonisKindnessEnabled = newValue;
                    },
                    () => SharedData.globalSettings.jonisKindnessEnabled,
                    "jonisKindness"),
                new BoolSetting("Blue Hive", "TheMathGeek314's Unlimited Hiveblood restores Lifeblood Masks",
                    newValue =>
                    {
                        SharedData.globalSettings.blueHiveEnabled = newValue;
                    },
                    () => SharedData.globalSettings.blueHiveEnabled,
                    "blueHive"),
            };

            Menu menuRef = new Menu("Charm Patch Settings", menuOptions);

            return menuRef.GetMenuScreen(modListMenu);
        }
    }

    internal class BoolSetting : HorizontalOption
    {
        public BoolSetting(string name, string description, Action<bool> applySetting, Func<bool> loadSetting, string id)
            : base(name, 
                  description, 
                  new string[] { "Yes", "No" }, 
                  (i) => applySetting(i == 0), 
                  () => loadSetting() ? 0 : 1, 
                  id) { }
    }
}