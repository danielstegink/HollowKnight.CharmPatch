using Modding;
using System;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    internal class GrubberflysReach : CharmPatch
    {
        public void AddHook()
        {
            ModHooks.ObjectPoolSpawnHook += ExtendBeam;
        }

        /// <summary>
        /// Grubberfly's Reach makes Longnail and Mark of Pride
        /// increase the range of beams from Grubberfly's Elegy
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private GameObject ExtendBeam(GameObject gameObject)
        {
            // We only want to modify the clones
            if (SharedData.globalSettings.grubberflysReachOn &&
                gameObject.name.StartsWith("Grubberfly Beam") &&
                gameObject.name.Contains("(Clone)"))
            {
                //SharedData.Log($"Grubberfly's Beam found: {gameObject.name}");
                string direction = GetDirection(gameObject.name);

                float modifier = GetModifier(direction);
                
                switch (direction)
                {
                    case "LEFT":
                    case "RIGHT":
                        float xScale = gameObject.transform.GetScaleX();
                        gameObject.transform.SetScaleX(xScale * modifier);
                        break;
                    case "UP":
                    case "DOWN":
                        float yScale = gameObject.transform.GetScaleY();
                        gameObject.transform.SetScaleY(yScale * modifier);
                        break;
                    default:
                        SharedData.Log($"Invalid direction for Grubberfly's Reach: {direction}");
                        break;
                }
            }

            return gameObject;
        }

        /// <summary>
        /// Gets the direction the beam is going
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private string GetDirection(string objectName)
        {
            if (objectName.StartsWith("Grubberfly BeamR"))
            {
                return "RIGHT";
            }
            else if (objectName.StartsWith("Grubberfly BeamL"))
            {
                return "LEFT";
            }
            else if (objectName.StartsWith("Grubberfly BeamU"))
            {
                return "UP";
            }
            else if (objectName.StartsWith("Grubberfly BeamD"))
            {
                return "DOWN";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets how much to increase the beam's range by
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private float GetModifier(string direction)
        {
            // If Longnail is equipped, we want to increase the range by 15%
            if (PlayerData.instance.equippedCharm_18 &&
                !PlayerData.instance.equippedCharm_38)
            {
                return 1.15f;
            }

            // If Mark of Pride is equipped, we want to increase the range by 25%
            if (!PlayerData.instance.equippedCharm_18 &&
                PlayerData.instance.equippedCharm_38)
            {
                // Unless we are swinging up, in which case the bonus is already
                // applied
                if (direction.Equals("UP"))
                {
                    return 1f;
                }

                return 1.25f;
            }

            // Both are equipped, we combine them to increase the range by 40%
            if (PlayerData.instance.equippedCharm_18 &&
                PlayerData.instance.equippedCharm_38)
            {
                // Again, unless we are already swinging up
                // In that case, we want to increase the range by 12%
                // That will multiply with the 25% already applied to
                // reach 40%
                if (direction.Equals("UP"))
                {
                    return 1.12f;
                }

                return 1.4f;
            }

            return 1f;
        }
    }
}