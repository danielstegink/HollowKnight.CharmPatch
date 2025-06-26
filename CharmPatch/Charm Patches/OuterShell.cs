using CharmPatch.OtherModHelpers;
using Modding;
using System.Collections;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    public class OuterShell : CharmPatch
    {
        /// <summary>
        /// Stores the Exaltation mod, if its installed
        /// </summary>
        public IMod exaltation;

        /// <summary>
        /// The number of extra hits Baldur Shell can take
        /// </summary>
        private int extraHits = 2;

        // The number of extra hits Baldur Shell has taken since the last refresh
        private int extraHitsTaken = 0;

        /// <summary>
        /// Tracks if Stone Shell is currently running
        /// </summary>
        private bool stoneShellActive = false;

        public void AddHook()
        {
            ModHooks.TakeHealthHook += Start;
            On.PlayerData.MaxHealth += Refresh;
        }

        /// <summary>
        /// Outer Shell lets Baldur Shell take 2 extra hits before breaking
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int Start(int damage)
        {
            //SharedData.Log("Attacked");
            if (SharedData.globalSettings.outerShellOn &&
                PlayerData.instance.blockerHits < 4 && 
                extraHitsTaken < extraHits &&
                PlayerData.instance.equippedCharm_5)
            {
                //SharedData.Log("Outer shell hit");
                PlayerData.instance.blockerHits++;
                extraHitsTaken++;

                // Set up a loop to heal until full
                if (!stoneShellActive)
                {
                    stoneShellActive = true;
                    GameManager.instance.StartCoroutine(StoneShell());
                }
            }

            return damage;
        }

        /// <summary>
        /// Coroutine that handles the regeneration of OuterShell hits in the background
        /// </summary>
        /// <returns></returns>
        private IEnumerator StoneShell()
        {
            while (CanHeal())
            {
                // Stone Shell waits 10 seconds to heal
                yield return new WaitForSeconds(10f);

                if (CanHeal())
                {
                    extraHitsTaken--;
                }
            }

            stoneShellActive = false;
            yield return new WaitForSeconds(0f);
        }

        /// <summary>
        /// Checks if OuterShell can regenerate
        /// </summary>
        /// <returns></returns>
        private bool CanHeal()
        {
            // If already healed, skip
            if (extraHitsTaken == 0)
            {
                return false;
            }

            // If the patch is not enabled, skip
            if (!SharedData.globalSettings.outerShellOn)
            {
                return false;
            }

            // Cancel if Outer Shell has been reset
            if (!stoneShellActive)
            {
                return false;
            }

            // If Theodore's Exaltation mod isn't installed, skip
            if (exaltation == null)
            {
                return false;
            }

            // Requires Stone Shell be unlocked and Baldur Shell equipped
            string modifierToken = Exaltation.GetProperty(SharedData.currentSave, "BaldurShellGlorified");
            bool stoneShellUnlocked = bool.Parse(modifierToken);
            if (!stoneShellUnlocked ||
                !PlayerData.instance.equippedCharm_5)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resets the number of extra hits taken to 0
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void Refresh(On.PlayerData.orig_MaxHealth orig, PlayerData self)
        {
            extraHitsTaken = 0;
            stoneShellActive = false;

            orig(self);
        }
    }
}
