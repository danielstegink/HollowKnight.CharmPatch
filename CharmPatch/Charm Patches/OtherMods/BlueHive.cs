using CharmPatch.OtherModHelpers;
using GlobalEnums;
using Modding;
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    public class BlueHive : CharmPatch
    {
        /// <summary>
        /// Stores the Unlimited Hiveblood mod, if its installed
        /// </summary>
        public IMod unlimitedHiveblood;

        /// <summary>
        /// Current max blue health
        /// </summary>
        private int maxBlue = 0;

        /// <summary>
        /// Tracks if Blue Hive is currently running
        /// </summary>
        private bool blueHiveActive = false;

        public void AddHook()
        {
            On.HeroController.TakeDamage += Start;
            ModHooks.CharmUpdateHook += Reset;
        }

        /// <summary>
        /// Hiveblood heals all masks when Unlimited Hiveblood is installed,
        /// even lifeblood masks
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="go"></param>
        /// <param name="damageSide"></param>
        /// <param name="damageAmount"></param>
        /// <param name="hazardType"></param>
        private void Start(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
        {
            //SharedData.Log($"Damage taken: {blueHiveActive}");

            // Store maximum blue masks
            //SharedData.Log("Setting max blue");
            int currentBlue = PlayerData.instance.healthBlue;
            if (maxBlue < currentBlue)
            {
                maxBlue = currentBlue;
            }

            // Apply damage
            orig(self, go, damageSide, damageAmount, hazardType);

            // Set up a loop to heal until full
            if (!blueHiveActive)
            {
                blueHiveActive = true;
                GameManager.instance.StartCoroutine(HealBlue());
            }
        }

        private IEnumerator HealBlue()
        {
            //SharedData.Log("Heal Blue started");

            while (CanHeal())
            {
                // Hiveblood waits 24 seconds to heal blue health normally
                yield return new WaitForSeconds(GetTimeout());

                if (CanHeal())
                {
                    //SharedData.Log("Blue Hive - Healing needed");
                    //int currentBlue = PlayerData.instance.healthBlue;
                    EventRegister.SendEvent("ADD BLUE HEALTH");
                    //SharedData.Log($"Blue health: {currentBlue} -> {PlayerData.instance.healthBlue}");
                }
            }

            blueHiveActive = false;
            yield return new WaitForSeconds(0f);
        }

        /// <summary>
        /// Reset all healing when resting at a bench
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        private void Reset(PlayerData data, HeroController controller)
        {
            maxBlue = 0;
            blueHiveActive = false;
        }

        /// <summary>
        /// Checks if the patch can heal the player
        /// </summary>
        /// <returns></returns>
        private bool CanHeal()
        {
            // If already healed, skip
            if (PlayerData.instance.healthBlue >= maxBlue)
            {
                //SharedData.Log($"{PlayerData.instance.healthBlue} blue health out of {maxBlue}");
                return false;
            }

            // If the patch is not enabled, skip
            if (!SharedData.globalSettings.blueHiveOn)
            {
                //SharedData.Log("Blue Hive not enabled");
                return false;
            }

            // If TheMathGeek314's Unlimited Hiveblood mod isn't installed, skip
            if (unlimitedHiveblood == null)
            {
                //SharedData.Log("Unlimited Hiveblood not found");
                return false;
            }

            // Unlimited Hiveblood only triggers if Hiveblood and Kingsoul/Void Heart are both equipped
            if (!PlayerData.instance.equippedCharm_36 || !PlayerData.instance.equippedCharm_29)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the timeout between lifeblood heals
        /// </summary>
        /// <returns></returns>
        private float GetTimeout()
        {
            // By default, Hiveblood takes 24 seconds to heal lifeblood
            float timeout = 24f;

            if (SharedData.charmChangerInstalled)
            {
                JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "hivebloodJonisTimer");
                timeout = float.Parse(modifierToken.ToString());
            }

            //SharedData.Log($"Hiveblood blue timeout: {timeout}");
            return timeout;
        }
    }
}