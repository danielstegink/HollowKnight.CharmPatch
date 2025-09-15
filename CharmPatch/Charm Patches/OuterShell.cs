using Modding;
using System.Collections;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Outer Shell increases the number of hits Baldur Shell can absorb
    /// </summary>
    public class OuterShell : Patch
    {
        public bool IsActive => SharedData.globalSettings.outerShellOn;

        public void Start()
        {
            if (IsActive)
            {
                ModHooks.TakeHealthHook += Start;
                On.PlayerData.MaxHealth += Refresh;
            }
        }

        public void Stop()
        {
            ModHooks.TakeHealthHook -= Start;
            On.PlayerData.MaxHealth -= Refresh;
        }

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

        /// <summary>
        /// Outer Shell lets Baldur Shell take 2 extra hits before breaking
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int Start(int damage)
        {
            if (extraHitsTaken < extraHits &&
                PlayerData.instance.GetBool("equippedCharm_5"))
            {
                PlayerData.instance.IntAdd("blockerHits", 1);
                extraHitsTaken++;

                // Set up a loop to heal until full
                if (!stoneShellActive)
                {
                    //CharmPatch.Instance.Log($"Starting Stone Shell");
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
                    //CharmPatch.Instance.Log($"Stone shell hits reduced to {extraHitsTaken}");
                }
            }

            stoneShellActive = false;
            yield return new WaitForSeconds(0f);
        }

        /// <summary>
        /// Checks if Outer Shell can regenerate
        /// </summary>
        /// <returns></returns>
        private bool CanHeal()
        {
            // If already healed, skip
            if (extraHitsTaken == 0)
            {
                //CharmPatch.Instance.Log($"Stone Shell - No hits to recharge");
                return false;
            }

            // If the patch is not enabled, skip
            if (!IsActive)
            {
                //CharmPatch.Instance.Log($"Stone Shell - Not turned on");
                return false;
            }

            // Cancel if Outer Shell has been reset
            if (!stoneShellActive)
            {
                //CharmPatch.Instance.Log($"Stone Shell - Reset");
                return false;
            }

            // If Theodore's Exaltation mod isn't installed, skip
            if (SharedData.exaltationMod == null)
            {
                //CharmPatch.Instance.Log($"Stone Shell - Exaltation not installed");
                return false;
            }

            // Requires Stone Shell be unlocked and Baldur Shell equipped
            if (!(bool)SharedData.dataStore["stoneShellUnlocked"] ||
                !PlayerData.instance.GetBool("equippedCharm_5"))
            {
                //CharmPatch.Instance.Log($"Stone Shell - Not upgraded or not equipped");
                return false;
            }

            //CharmPatch.Instance.Log($"Stone Shell - Able to heal");
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
