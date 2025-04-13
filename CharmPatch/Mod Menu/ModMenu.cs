using Modding;
using System;
using System.Collections.Generic;

namespace CharmPatch
{
    public static class ModMenu
    {
        public static List<IMenuMod.MenuEntry> CreateMenu()
        {
            SharedData.Log("Building menu");

            List<IMenuMod.MenuEntry> menuOptions = new List<IMenuMod.MenuEntry>() 
            { 
                new IMenuMod.MenuEntry()
                {
                    Name = "Berserk's Fury",
                    Description = "Chance for Fury of the Fallen to ignore damage while active",
                    Values = MenuValues(),
                    Saver = value => SharedData.globalSettings.berserkersFuryOn = Convert.ToBoolean(value),
                    Loader = () => Convert.ToInt32(SharedData.globalSettings.berserkersFuryOn)
                },
                new IMenuMod.MenuEntry()
                {
                    Name = "Blue Hive",
                    Description = "TheMathGeek314's Unlimited Hiveblood restores Lifeblood Masks",
                    Values = MenuValues(),
                    Saver = value => SharedData.globalSettings.blueHiveOn = Convert.ToBoolean(value),
                    Loader = () => Convert.ToInt32(SharedData.globalSettings.blueHiveOn)
                },
                new IMenuMod.MenuEntry()
                {
                    Name = "Critical Blow",
                    Description = "Nail Art damage increased when Heavy Blow equipped",
                    Values = MenuValues(),
                    Saver = value => SharedData.globalSettings.criticalBlowOn = Convert.ToBoolean(value),
                    Loader = () => Convert.ToInt32(SharedData.globalSettings.criticalBlowOn)
                },
                new IMenuMod.MenuEntry()
                {
                    Name = "Dark Dashmaster",
                    Description = "Cooldown of Shadow Dash reduced when Dashmaster equipped",
                    Values = MenuValues(),
                    Saver = value => SharedData.globalSettings.darkDashmasterOn = Convert.ToBoolean(value),
                    Loader = () => Convert.ToInt32(SharedData.globalSettings.darkDashmasterOn)
                },
                new IMenuMod.MenuEntry()
                {
                    Name = "Joni's Kindness",
                    Description = "Joni's Blessing gives more Lifeblood Masks",
                    Values = MenuValues(),
                    Saver = value => SharedData.globalSettings.jonisKindnessOn = Convert.ToBoolean(value),
                    Loader = () => Convert.ToInt32(SharedData.globalSettings.jonisKindnessOn)
                },
                new IMenuMod.MenuEntry()
                {
                    Name = "Mantis Arts",
                    Description = "Nail Art range increased when Longnail and/or Mark of Pride equipped",
                    Values = MenuValues(),
                    Saver = value => SharedData.globalSettings.mantisArtsOn = Convert.ToBoolean(value),
                    Loader = () => Convert.ToInt32(SharedData.globalSettings.mantisArtsOn)
                },
                new IMenuMod.MenuEntry()
                {
                    Name = "Merciful Melody",
                    Description = "Chance to heal when Carefree Melody is triggered",
                    Values = MenuValues(),
                    Saver = value => SharedData.globalSettings.mercifulMelodyOn = Convert.ToBoolean(value),
                    Loader = () => Convert.ToInt32(SharedData.globalSettings.mercifulMelodyOn)
                },
            };

            return menuOptions;
        }

        private static string[] MenuValues()
        {
            return new string[] { "OFF", "ON" };
        }
    }
}