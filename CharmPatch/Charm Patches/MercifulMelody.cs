using Modding;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    public class MercifulMelody : CharmPatch
    {
        public void AddHook()
        {
            ModHooks.TakeHealthHook += Start;
        }

        /// <summary>
        /// Adds a chance that Carefree Melody will heal 1 damage when triggered
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private int Start(int damage)
        {
            if (SharedData.globalSettings.mercifulMelodyOn)
            {
                // Only trigger when Carefree Melody blocks and player is damaged
                GameObject shield = HeroController.instance.carefreeShield;
                if (shield != null && 
                    shield.activeSelf &&
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
            }

            return damage;
        }
    }
}