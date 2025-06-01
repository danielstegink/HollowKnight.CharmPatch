using Satchel.BetterMenus;
using System;
using System.Collections.Generic;

namespace CharmPatch
{
    public static class ModMenu
    {
        private static Menu menu;
        private static MenuScreen menuScreen;

        public static Dictionary<string, MenuScreen> subMenus;

        public static MenuScreen CreateMenuScreen(MenuScreen modListMenu)
        {
            // Declare the Charm Patch menu
            //SharedData.Log("Building root menu");
            menu = new Menu("Charm Patch Options", new Element[]{});

            // Populate Charm Patch menu
            PopulateCharmPatchMenu();

            // Insert the Charm Patch menu into the overall menu
            //SharedData.Log("Integrating root menu");
            menuScreen = menu.GetMenuScreen(modListMenu);

            // Populate the sub-menus
            PopulateSubMenus();

            //SharedData.Log("Returning menu");
            return menuScreen;
        }

        /// <summary>
        /// Adds buttons and elements to the Charm Patch menu
        /// </summary>
        private static void PopulateCharmPatchMenu()
        {
            //SharedData.Log("Populating root menu");

            // Populate root menu
            menu.AddElement(new HorizontalOption("Berserk's Fury",
                    "Chance for Fury of the Fallen to ignore damage while active",
                    MenuValues(),
                    value => SharedData.globalSettings.berserkersFuryOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.berserkersFuryOn)));

            menu.AddElement(new HorizontalOption("Dark Dashmaster",
                    "Cooldown of Shadow Dash reduced when Dashmaster equipped",
                    MenuValues(),
                    value => SharedData.globalSettings.darkDashmasterOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.darkDashmasterOn)));

            menu.AddElement(new HorizontalOption("Joni's Kindness",
                    "Joni's Blessing gives more Lifeblood Masks",
                    MenuValues(),
                    value => SharedData.globalSettings.jonisKindnessOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.jonisKindnessOn)));

            menu.AddElement(new HorizontalOption("Merciful Melody",
                    "Chance to heal when Carefree Melody is triggered",
                    MenuValues(),
                    value => SharedData.globalSettings.mercifulMelodyOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.mercifulMelodyOn)));

            menu.AddElement(new HorizontalOption("Outer Shell",
                    "Baldur Shell takes additional hits before breaking",
                    MenuValues(),
                    value => SharedData.globalSettings.outerShellOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.outerShellOn)));

            // The sub-menus have to be populated retroactively
            menu.AddElement(Blueprints.NavigateToMenu("Grubberfly's Elegy",
                    "",
                    () => subMenus["Grubberfly's Elegy"]));

            menu.AddElement(Blueprints.NavigateToMenu("Nail Arts",
                    "",
                    () => subMenus["Nail Arts"]));

            menu.AddElement(Blueprints.NavigateToMenu("Mod Integrations",
                    "",
                    () => subMenus["Mod Integrations"]));
        }

        // Builds the sub-menus so they can be referenced
        private static void PopulateSubMenus()
        {
            //SharedData.Log("Populating sub-menus");
            subMenus = new Dictionary<string, MenuScreen>();

            Menu grubberflyMenu = new Menu("Grubberfly's Elegy", new Element[]
            {
                new HorizontalOption("Grubberfly's Reach",
                    "Grubberfly's Elegy affected by Longnail and Mark of Pride",
                    MenuValues(),
                    value => SharedData.globalSettings.grubberflysReachOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.grubberflysReachOn)),
                new HorizontalOption("Joni's Elegy",
                    "Grubberfly's Elegy attack triggers when the player is at full health",
                    MenuValues(),
                    value => SharedData.globalSettings.jonisElegyOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.jonisElegyOn)),
            });
            subMenus.Add("Grubberfly's Elegy", grubberflyMenu.GetMenuScreen(menuScreen));

            Menu nailArtsMenu = new Menu("Nail Arts", new Element[]
            {
                new HorizontalOption("Critical Blow",
                    "Nail Art damage increased by Heavy Blow",
                    MenuValues(),
                    value => SharedData.globalSettings.criticalBlowOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.criticalBlowOn)),
                new HorizontalOption("Mantis Arts",
                    "Nail Art range affected by Longnail and Mark of Pride",
                    MenuValues(),
                    value => SharedData.globalSettings.mantisArtsOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.mantisArtsOn)),
                new HorizontalOption("Mighty Arts",
                    "Nail Art damage affected by Fragile/Unbreakable Strength",
                    MenuValues(),
                    value => SharedData.globalSettings.mightyArtsOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.mightyArtsOn)),
                new HorizontalOption("Quick Arts",
                    "Nail Art charge time reduced by Quick Slash",
                    MenuValues(),
                    value => SharedData.globalSettings.quickArtsOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.quickArtsOn)),
            });
            subMenus.Add("Nail Arts", nailArtsMenu.GetMenuScreen(menuScreen));

            Menu otherModMenu = new Menu("Mod Integrations", new Element[]
            {
                new HorizontalOption("Blue Hive",
                    "TheMathGeek314's Unlimited Hiveblood restores Lifeblood Masks",
                    MenuValues(),
                    value => SharedData.globalSettings.blueHiveOn = Convert.ToBoolean(value),
                    () => Convert.ToInt32(SharedData.globalSettings.blueHiveOn))
            });
            subMenus.Add("Mod Integrations", otherModMenu.GetMenuScreen(menuScreen));
        }

        private static string[] MenuValues()
        {
            return new string[] { "OFF", "ON" };
        }
    }
}