﻿using CharmPatch.OtherModHelpers;
using Modding;
using Newtonsoft.Json.Linq;
using Satchel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    internal class GrubberflysReach : CharmPatch
    {
        /// <summary>
        /// Stores beam dimensions to fight scale-stacking
        /// </summary>
        private Dictionary<string, Vector3> baseDimensions = new Dictionary<string, Vector3>();

        public void AddHook()
        {
            ModHooks.ObjectPoolSpawnHook += ExtendBeam;
        }

        /// <summary>
        /// Grubberfly's Reach makes Longnail and Mark of Pride increase the range of beams 
        /// from Grubberfly's Elegy
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private GameObject ExtendBeam(GameObject gameObject)
        {
            // We only want to modify the clones 
            if (SharedData.globalSettings.grubberflysReachOn &&
                gameObject.name.StartsWith("Grubberfly Beam") &&
                gameObject.name.Contains("(Clone)"))
            {
                //SharedData.Log($"Grubberfly's Beam found: {gameObject.name}");
                //SharedData.Log($"Old dimensions: {gameObject.transform.GetScaleX()}, {gameObject.transform.GetScaleY()}");

                // Risk of buff getting stacked and beams getting ridiculously big
                // So store the dimensions of the first one we see of each type, 
                //  and auto-correct each on to that scale before continuing
                if (!baseDimensions.Keys.Contains(gameObject.name))
                {
                    //SharedData.Log("Adding beam to list");
                    baseDimensions.Add(gameObject.name, gameObject.transform.localScale);
                }
                else
                {
                    gameObject.transform.localScale = baseDimensions[gameObject.name];
                }

                string direction = GetDirection(gameObject.name);
                float modifier = GetModifier(direction);
                //SharedData.Log($"Grubberfly's Reach modifier: {modifier}");

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

                //SharedData.Log($"Final dimensions: {gameObject.transform.GetScaleX()}, {gameObject.transform.GetScaleY()}");
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
                !PlayerData.instance.equippedCharm_13)
            {
                if (SharedData.charmChangerInstalled)
                {
                    JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "longnailScale");
                    float modifier = float.Parse(modifierToken.ToString()) / 100;
                    return 1 + modifier;
                }

                return 1.15f;
            }

            // If Mark of Pride is equipped, we want to increase the range by 25%
            if (!PlayerData.instance.equippedCharm_18 &&
                PlayerData.instance.equippedCharm_13)
            {
                // Unless we are swinging up, in which case the bonus is already
                // applied
                if (direction.Equals("UP"))
                {
                    return 1f;
                }

                if (SharedData.charmChangerInstalled)
                {
                    JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "markOfPrideScale");
                    float modifier = float.Parse(modifierToken.ToString()) / 100;
                    return 1 + modifier;
                }

                return 1.25f;
            }

            // Both are equipped, we combine them to increase the range by 40%
            if (PlayerData.instance.equippedCharm_18 &&
                PlayerData.instance.equippedCharm_13)
            {
                // Again, unless we are already swinging up
                // In that case, we want to increase the range by 12%
                // That will multiply with the 25% already applied to
                // reach 40%
                if (direction.Equals("UP"))
                {
                    if (SharedData.charmChangerInstalled)
                    {
                        // Normal: 1.25 * 1.12 ~= 1.4
                        // New: m(MOP) * x = m(Both)
                        JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "markOfPrideScale");
                        float mopModifier = 1 + float.Parse(modifierToken.ToString()) / 100;

                        modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "longnailMarkOfPrideScale");
                        float bothModifier = 1 + float.Parse(modifierToken.ToString()) / 100;

                        return bothModifier / mopModifier;
                    }

                    return 1.12f;
                }

                if (SharedData.charmChangerInstalled)
                {
                    JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "longnailMarkOfPrideScale");
                    float modifier = float.Parse(modifierToken.ToString()) / 100;
                    return 1 + modifier;
                }

                return 1.4f;
            }

            return 1f;
        }
    }
}