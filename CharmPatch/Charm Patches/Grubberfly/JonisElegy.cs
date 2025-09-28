using GlobalEnums;
using HKMirror.Reflection.SingletonClasses;
using System;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Joni's Elegy makes it possible for Grubberfly's Elegy to start working again after 
    /// taking damage while Joni's Blessing is equipped if the damage appears to have healed
    /// </summary>
    public class JonisElegy : Patch
    {
        public bool IsActive => SharedData.globalSettings.jonisElegyOn;

        public void Start()
        {
            On.HeroController.Attack += KeepGrubberfly;
        }

        /// <summary>
        /// When we attack, if our blue health appears to match what we'd expect from Joni, we enabled Grubberfly's Elegy
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="attackDir"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void KeepGrubberfly(On.HeroController.orig_Attack orig, HeroController self, AttackDirection attackDir)
        {
            if (IsActive)
            {
                //CharmPatch.Instance.Log($"Current health: {PlayerData.instance.GetInt("healthBlue")} out of {PlayerData.instance.GetInt("joniHealthBlue")}");
                if (PlayerData.instance.GetInt("healthBlue") >= PlayerData.instance.GetInt("joniHealthBlue"))
                {
                    HeroControllerR.joniBeam = true;
                    //CharmPatch.Instance.Log("Joni enabled");
                }
                else if (PlayerData.instance.GetInt("healthBlue") < PlayerData.instance.GetInt("joniHealthBlue"))
                {
                    HeroControllerR.joniBeam = false;
                    //CharmPatch.Instance.Log("Joni disabled");
                }
            }

            orig(self, attackDir);
        }
    }
}