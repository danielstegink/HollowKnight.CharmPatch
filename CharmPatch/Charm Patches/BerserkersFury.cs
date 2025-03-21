using Modding;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    public class BerserkersFury : CharmPatch
    {
        public void AddHook()
        {
            ModHooks.TakeHealthHook += Start;
        }

        /// <summary>
        /// Adds a chance for Fury of the Fallen to ignore damage while triggered
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int Start(int damage)
        {
            if (SharedData.globalSettings.berserkersFuryOn)
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
            }

            return damage;
        }
    }
}