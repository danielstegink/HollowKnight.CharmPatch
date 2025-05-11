using GlobalEnums;
using HKMirror.Reflection.SingletonClasses;
using System;

namespace CharmPatch.Charm_Patches
{
    internal class JonisElegy : CharmPatch
    {
        public void AddHook()
        {
            On.HeroController.Attack += KeepGrubberfly;
        }

        /// <summary>
        /// Normally, Grubberfly's Elegy doesn't trigger while Joni's Blessing is equipped and the player has
        /// taken damage, even if Hiveblood has healed the damage already. This patch should fix that.
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="attackDir"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void KeepGrubberfly(On.HeroController.orig_Attack orig, HeroController self, AttackDirection attackDir)
        {
            //SharedData.Log($"Current health: {PlayerData.instance.healthBlue} out of {PlayerData.instance.joniHealthBlue}");
            if (SharedData.globalSettings.jonisElegyOn)
            {
                if (PlayerData.instance.healthBlue == PlayerData.instance.joniHealthBlue)
                {
                    HeroControllerR.joniBeam = true;
                    //SharedData.Log("Joni enabled");
                }
                else if (PlayerData.instance.healthBlue < PlayerData.instance.joniHealthBlue)
                {
                    HeroControllerR.joniBeam = false;
                    //SharedData.Log("Joni disabled");
                }
            }
            
            orig(self, attackDir);
        }
    }
}