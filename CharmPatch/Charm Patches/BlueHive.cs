using Modding;
using System.Threading.Tasks;

namespace CharmPatch.Charm_Patches
{
    public class BlueHive : CharmPatch
    {
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
            ModHooks.TakeHealthHook += Start;
            ModHooks.CharmUpdateHook += Reset;
        }

        /// <summary>
        /// Hiveblood heals all Lifeblood Masks when Unlimited Hiveblood is installed
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int Start(int damage)
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
            while (CanHeal())
            {
                //SharedData.Log("Blue Hive - Healing needed");

                // Hiveblood waits 24 seconds to heal blue health normally
                System.Threading.Thread.Sleep(24 * 1000);

                if (CanHeal())
                {
                    PlayerData.instance.healthBlue += 1;
                }
            }

            blueHiveActive = false;
            //SharedData.Log("Blue Hive - Healing not needed");
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
                return false;
            }

            // If the patch is not enabled, skip
            if (!SharedData.globalSettings.blueHiveOn)
            {
                return false;
            }

            // If TheMathGeek314's Unlimited Hiveblood mod isn't installed, skip
            if (ModHooks.GetMod("UnlimitedHiveblood") == null)
            {
                return false;
            }

            return true;
        }
    }
}