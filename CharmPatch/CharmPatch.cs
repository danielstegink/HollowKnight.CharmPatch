using HutongGames.PlayMaker;
using Modding;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CharmPatch
{
    public class CharmPatch : Mod
    {
        public override string GetVersion() => "1.0.0.0";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            SharedData.Log("Starting mod");

            ModHooks.CharmUpdateHook += DarkDashmaster;

            // Reduce Dreamshield cost to 2
            PlayerData.instance.charmCost_38 = 2;

            ModHooks.HeroUpdateHook += FerociousGrimmchild;

            ModHooks.TakeHealthHook += BerserkersFury;

            ModHooks.TakeHealthHook += MercifulMelody;

            ModHooks.TakeHealthHook += OuterShell;
            ModHooks.CharmUpdateHook += ResetOuterShell;

            ModHooks.CharmUpdateHook += JonisKindness;

            ModHooks.FinishedLoadingModsHook += LoadModPatches;

            SharedData.Log("Startup complete");
        }

        #region Dashmaster
        /// <summary>
        /// Stores the original cooldown
        /// </summary>
        private float baseCooldown = 0f;

        /// <summary>
        /// Reduces the cooldown of Shadow dash when Dashmaster is equipped
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        private void DarkDashmaster(PlayerData data, HeroController controller)
        {
            // Buff once, and only if Dashmaster is equipped
            if (data.equippedCharm_31 && baseCooldown == 0)
            {
                baseCooldown = controller.SHADOW_DASH_COOLDOWN;

                // Dashmaster reduces normal dash cooldown by 33%
                float newCooldown = baseCooldown * 0.67f;
                controller.SHADOW_DASH_COOLDOWN = newCooldown;
                //SharedData.Log($"Shadow dash: {baseCooldown} -> {newCooldown}");
            }

            // Make sure to reset if Dashmaster removed
            if (!data.equippedCharm_31 && baseCooldown != 0)
            {
                controller.SHADOW_DASH_COOLDOWN = baseCooldown;
                //SharedData.Log($"Shadow dash reset to {baseCooldown}");

                baseCooldown = 0f;
            }
        }
        #endregion

        #region Grimmchild
        private List<GameObject> buffedGrimmchildren = new List<GameObject>();

        /// <summary>
        /// Reduces the wait time between Grimmchild's attacks
        /// </summary>
        private void FerociousGrimmchild()
        {
            List<GameObject> gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>()
                                            .Where(x => x.name.StartsWith("Grimmchild"))
                                            .ToList();
            foreach (GameObject gameObject in gameObjects)
            {
                // Skip if already buffed
                if (buffedGrimmchildren.Contains(gameObject))
                {
                    continue;
                }

                // All Grimchild data is stored in the Control FSM
                PlayMakerFSM fsm = FSMUtility.LocateFSM(gameObject, "Control");

                // The wait time between attacks is a float value in the No Target state
                float waitTime = SFCore.Utils.FsmUtil.GetAction<HutongGames.PlayMaker.Actions.SetFloatValue>(fsm, "No Target", 0).floatValue.Value;

                // Reduce the wait time to 2/3
                float newTime = waitTime * 2f / 3f;
                SFCore.Utils.FsmUtil.GetAction<HutongGames.PlayMaker.Actions.SetFloatValue>(fsm, "No Target", 0).floatValue.Value = newTime;
                //SharedData.Log($"Grimmchild wait time reduced: {waitTime} -> {newTime}");

                // Add Grimmchild to list of buffed pets
                buffedGrimmchildren.Add(gameObject);
            }
        }
        #endregion

        /// <summary>
        /// Adds a chance for Fury of the Fallen to ignore damage while triggered
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int BerserkersFury(int damage)
        {
            // Confirm Fury is active 
            GameObject charmEffectsObject = HeroController.instance.gameObject.transform.Find("Charm Effects").gameObject;
            PlayMakerFSM fury = charmEffectsObject.LocateMyFSM("Fury");
            //SharedData.Log($"Fury state: {fury.ActiveStateName}");

            if (fury.ActiveStateName == "HP Pause") // The state just before we die
            {
                // Get a random number between 1 and 100
                int random = UnityEngine.Random.Range(1, 101);
                //SharedData.Log($"Berserker's Fury - {random}");

                // 20% chance we ignore all damage
                if (random <= 20)
                {
                    //SharedData.Log("Berserker's Fury - Damage ignored");
                    return 0;
                }
            }

            return damage;
        }

        /// <summary>
        /// Adds a chance that Carefree Melody will heal 1 damage when triggered
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int MercifulMelody(int damage)
        {
            // Only trigger when Carefree Melody blocks and player is damaged
            GameObject shield = HeroController.instance.carefreeShield;
            if (shield != null && shield.activeSelf && 
                PlayerData.instance.health < PlayerData.instance.maxHealth)
            {
                int random = UnityEngine.Random.Range(1, 101);
                //SharedData.Log($"Merciful Melody - {random}");

                // 50% chance we gain 1 Mask
                if (random <= 50)
                {
                    HeroController.instance.AddHealth(1);
                    //SharedData.Log("Merciful Melody - 1 Mask restored");
                }
            }

            return damage;
        }

        #region Baldur Shell
        int hitsAbsorbed = 0;

        /// <summary>
        /// Increases the number of hits Baldur Shell can take by 2
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int OuterShell(int damage)
        {
            if (PlayerData.instance.blockerHits < 4 && hitsAbsorbed < 2)
            {
                PlayerData.instance.blockerHits = 4;
                hitsAbsorbed++;
            }

            return damage;
        }

        /// <summary>
        /// Outer Shell resets when you rest at a bench, just like Baldur Shell
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        private void ResetOuterShell(PlayerData data, HeroController controller)
        {
            hitsAbsorbed = 0;
        }
        #endregion

        /// <summary>
        /// Joni's Blessing gives 2 additional Masks
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        private void JonisKindness(PlayerData data, HeroController controller)
        {
            if (PlayerData.instance.equippedCharm_27)
            {
                PlayerData.instance.joniHealthBlue += 2;
                PlayerData.instance.MaxHealth();
                //SharedData.Log($"Joni count: {PlayerData.instance.joniHealthBlue}");
            }
        }

        private void LoadModPatches()
        {
            // If Unlimited Hiveblood is installed, apply its effects to the Lifeblood
            if (ModHooks.GetMod("UnlimitedHiveblood") != null)
            {
                //SharedData.Log("Unlimited Hiveblood detected");
                ModHooks.TakeHealthHook += BlueHive;
                ModHooks.CharmUpdateHook += ResetBlueHive;
            }
        }

        #region Blue Hive
        /// <summary>
        /// Current max blue health
        /// </summary>
        private int maxBlue = 0;

        /// <summary>
        /// Tracks if Blue Hive is currently running
        /// </summary>
        private bool blueHiveActive = false;

        /// <summary>
        /// Hiveblood heals Lifeblood Masks
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int BlueHive(int damage)
        {
            // Confirm Hiveblood equipped
            if (PlayerData.instance.equippedCharm_36)
            {
                //SharedData.Log($"Blue Hive - {damage} damage taken");

                int currentBlue = PlayerData.instance.healthBlue;
                if (maxBlue < currentBlue)
                {
                    maxBlue = currentBlue;
                }
                //SharedData.Log($"Blue Hive - Max Blue {maxBlue}");

                // Run as a separate thread so it doesn't slow down regular processes
                new Task(HealBlue).Start();
            }

            return damage;
        }

        /// <summary>
        /// Turn Blue Hive off
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        private void ResetBlueHive(PlayerData data, HeroController controller)
        {
            maxBlue = 0;
            blueHiveActive = false;
        }

        /// <summary>
        /// Heals Lifeblood Masks
        /// </summary>
        private void HealBlue()
        {
            // If Blue Hive already active, skip
            if (blueHiveActive)
            {
                return;
            }
            blueHiveActive = true;

            //SharedData.Log($"Blue Hive - Current Blue {PlayerData.instance.healthBlue}");
            while (PlayerData.instance.healthBlue < maxBlue && blueHiveActive)
            {
                //SharedData.Log("Blue Hive - Healing needed");

                // Hivesong waits 24 seconds to heal blue health normally
                System.Threading.Thread.Sleep(24 * 1000);

                if (PlayerData.instance.healthBlue < maxBlue && blueHiveActive)
                {
                    //int currentBlue = PlayerData.instance.healthBlue;
                    PlayerData.instance.healthBlue += 1;
                    //SharedData.Log($"Blue Hive - {currentBlue} -> {PlayerData.instance.healthBlue}");
                }
            }

            blueHiveActive = false;
            //SharedData.Log("Blue Hive - Healing not needed");
        }
        #endregion
    }
}