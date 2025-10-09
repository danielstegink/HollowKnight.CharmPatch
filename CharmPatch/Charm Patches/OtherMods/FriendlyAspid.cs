using GlobalEnums;
using System.Linq;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// Friendly Aspid makes it so Nickc01's Aspid Aspect doesn't damage the player
    /// </summary>
    public class FriendlyAspid : Patch
    {
        public bool IsActive => SharedData.globalSettings.friendlyAspidOn;

        public void Start()
        {
            On.HeroController.TakeDamage += Block;
        }

        /// <summary>
        /// Focusing on the damage step means the player can still "block" shots, but allows us to negate damage with minimal invasion of the code
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="go"></param>
        /// <param name="damageSide"></param>
        /// <param name="damageAmount"></param>
        /// <param name="hazardType"></param>
        private void Block(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType)
        {
            if (IsActive &&
                SharedData.ancientAspidMod != null &&
                go.name.StartsWith("Aspid Shot - Damage All"))
            {
                Component friendlyAspidComponent = go.GetComponentsInChildren<Component>()
                                                        .Where(x => x.GetType().Name.Equals("AspidShotExtraDamager"))
                                                        .FirstOrDefault();
                if (friendlyAspidComponent != default)
                {
                    //CharmPatch.Instance.Log($"Friendly Aspid - AspidShotExtraDamager found");
                    damageAmount = 0;
                }
            }

            orig(self, go, damageSide, damageAmount, hazardType);
        }
    }
}