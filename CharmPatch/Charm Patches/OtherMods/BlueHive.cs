using GlobalEnums;
using HKMirror.Reflection.SingletonClasses;
using Modding;
using System.Collections;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Blue Hive supplements Unlimited Hiveblood by regenerating lifeblood masks
    /// </summary>
    public class BlueHive : Patch
    {
        public bool IsActive => SharedData.globalSettings.blueHiveOn;

        public void Start()
        {
            if (IsActive)
            {
                On.HeroController.TakeDamage += StartHealing;
                ModHooks.CharmUpdateHook += Reset;
            }
        }

        public void Stop()
        {
            On.HeroController.TakeDamage -= StartHealing;
            ModHooks.CharmUpdateHook -= Reset;
        }

        /// <summary>
        /// Tracks if Blue Hive is currently running
        /// </summary>
        private bool blueHiveActive = false;

        #region StartHealing
        /// <summary>
        /// Starts the healing process
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="go"></param>
        /// <param name="damageSide"></param>
        /// <param name="damageAmount"></param>
        /// <param name="hazardType"></param>
        private void StartHealing(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
        {
            // To prevent this from running in rapid succession and creating duplicate threads, use CanTakeDamage to check for I-Frames
            bool canTakeDamage = HeroControllerR.CanTakeDamage();
            orig(self, go, damageSide, damageAmount, hazardType);

            // Set up a loop to heal until full
            if (!blueHiveActive &&
                canTakeDamage)
            {
                //CharmPatch.Instance.Log($"Blue Hive - Starting");
                blueHiveActive = true;
                GameManager.instance.StartCoroutine(HealBlue());
            }
        }

        /// <summary>
        /// Passively regenerates lifeblood masks in the background, simulating Hiveblood
        /// </summary>
        /// <returns></returns>
        private IEnumerator HealBlue()
        {
            while (CanHeal())
            {
                yield return new WaitForSeconds(GetTimeout());

                if (CanHeal())
                {
                    EventRegister.SendEvent("ADD BLUE HEALTH");
                    //CharmPatch.Instance.Log($"Blue Hive - 1 health restored");
                }
            }

            blueHiveActive = false;
            yield return new WaitForSeconds(0f);
            //CharmPatch.Instance.Log($"Blue Hive - Stopping");
        }

        /// <summary>
        /// Checks if the patch can heal the player
        /// </summary>
        /// <returns></returns>
        private bool CanHeal()
        {
            // If already healed, skip
            if (PlayerData.instance.GetInt("healthBlue") >= PlayerData.instance.GetInt("joniHealthBlue"))
            {
                //CharmPatch.Instance.Log($"Blue Hive - {PlayerData.instance.GetInt("healthBlue")} is more than {PlayerData.instance.GetInt("joniHealthBlue")}");
                return false;
            }

            // If the patch is not enabled, skip
            if (!IsActive)
            {
                return false;
            }

            // Cancel if Blue Hive has been reset
            if (!blueHiveActive)
            {
                return false;
            }

            // If TheMathGeek314's Unlimited Hiveblood mod isn't installed, skip
            if (SharedData.unlimitedHivebloodMod == null)
            {
                return false;
            }

            // Unlimited Hiveblood only triggers if Hiveblood and Kingsoul/Void Heart are both equipped
            if (!PlayerData.instance.GetBool("equippedCharm_36") || 
                !PlayerData.instance.GetBool("equippedCharm_29"))
            {
                return false;
            }

            //CharmPatch.Instance.Log($"Blue Hive - Can heal");
            return true;
        }

        /// <summary>
        /// By default, Hiveblood takes 24 seconds to heal lifeblood
        /// </summary>
        /// <returns></returns>
        private float GetTimeout()
        {
            if (SharedData.charmChangerMod != null)
            {
                return (float)SharedData.dataStore["hiveblood"];
            }

            return 24f;
        }
        #endregion

        /// <summary>
        /// Reset all healing when resting at a bench
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        private void Reset(PlayerData data, HeroController controller)
        {
            blueHiveActive = false;
        }
    }
}