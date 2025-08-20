using DanielSteginkUtils.Utilities;
using GlobalEnums;
using HKMirror.Reflection.SingletonClasses;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Merciful Melody adds a chance of Carefree Melody healing 1 health when triggered
    /// </summary>
    public class MercifulMelody : Patch
    {
        public bool IsActive => SharedData.globalSettings.mercifulMelodyOn;

        public void Start()
        {
            if (IsActive)
            {
                On.HeroController.TakeDamage += Heal;
            }
        }

        public void Stop()
        {
            On.HeroController.TakeDamage -= Heal;
        }

        /// <summary>
        /// Personally, I think this ability should be equivalent to a 5% increase in CM's chance of triggering.
        /// 
        /// Healing a mask negates damage, making it equivalent to CM triggering twice in a row, so there should 
        /// be a roughly 6.5% chance of a heal occurring.
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="go"></param>
        /// <param name="damageSide"></param>
        /// <param name="damageAmount"></param>
        /// <param name="hazardType"></param>
        private void Heal(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
        {
            // We don't want to repeatedly trigger while I-Frames are active
            // So we check before hand if we could've even taken damage
            // But we have to check before the call, or I-Frames will be active and we'll get a false negative
            bool canTakeDamage = HeroControllerR.CanTakeDamage();
            orig(self, go, damageSide, damageAmount, hazardType);

            // Only trigger when Carefree Melody blocks
            GameObject shield = HeroController.instance.carefreeShield;
            if (shield != null &&
                shield.activeSelf)
            {
                // Verify that we even have damage to heal and that
                // I-Frames aren't active and making this trigger repeatedly
                if (PlayerData.instance.GetInt("health") < PlayerData.instance.CurrentMaxHealth &&
                    canTakeDamage)
                {
                    int healingChance = Calculations.GetSecondMelodyShield(5f);
                    int random = UnityEngine.Random.Range(1, 101);
                    //CharmPatch.Instance.Log($"Merciful Melody - {random} vs {healingChance}");
                    if (random <= healingChance)
                    {
                        HeroController.instance.AddHealth(1);
                    }
                }
            }
        }
    }
}