using Satchel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    internal class MantisArts : CharmPatch
    {
        public void AddHook()
        {
            On.HeroController.Update += Start;
        }

        /// <summary>
        /// List of the object names of the Nail Art attacks
        /// </summary>
        private List<string> nailArtNames = new List<string>()
        {
            "Cyclone Slash",
            "Great Slash",
            "Dash Slash",
            "Hit L",
            "Hit R"
        };

        /// <summary>
        /// Stores the original scales of the nail arts
        /// </summary>
        private Dictionary<string, Vector3> originalNailArts = new Dictionary<string, Vector3>();

        private void Start(On.HeroController.orig_Update orig, HeroController self)
        {
            // Get all nail art objects
            List<GameObject> gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>()
                                            .AsEnumerable()
                                            .Where(x => nailArtNames.Contains(x.name))
                                            .ToList();

            // If patch is active, apply buffs
            if (SharedData.globalSettings.mantisArtsOn)
            {
                float modifier = GetModifier();

                foreach (GameObject gameObject in gameObjects)
                {
                    //SharedData.Log($"Applying Mantis Arts ({modifier}) to {gameObject.name}");

                    StoreOriginal(gameObject);

                    Vector3 currentScale = gameObject.transform.localScale;
                    Vector3 originalScale = originalNailArts[gameObject.name];
                    gameObject.SetScale(originalScale.x * modifier, originalScale.y * modifier);

                    Vector3 newScale = gameObject.transform.localScale;
                    //SharedData.Log($"{gameObject} buffed: {ToString(currentScale)} -> {ToString(originalScale)} -> {ToString(newScale)}");
                }
            }
            else // If patch is not active, remove buffs
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    StoreOriginal(gameObject);

                    Vector3 currentScale = gameObject.transform.localScale;
                    Vector3 originalScale = originalNailArts[gameObject.name];
                    gameObject.SetScale(originalScale.x, originalScale.y);

                    //SharedData.Log($"{gameObject} reset: {ToString(currentScale)} -> {ToString(originalScale)}");
                }
            }

            orig(self);
        }

        /// <summary>
        /// Gets how much to increase the range by
        /// </summary>
        /// <returns></returns>
        private float GetModifier()
        {
            float modifier = 1;

            // Longnail increases nail range by 15%
            if (PlayerData.instance.equippedCharm_18)
            {
                modifier += 0.15f;
            }

            // Mark of Pride increases nail range by 25%
            // The two stack on each other, so equipping both increases range by 40%
            if (PlayerData.instance.equippedCharm_13)
            {
                modifier += 0.25f;
            }

            return modifier;
        }

        /// <summary>
        /// Stores the nail art object's original size
        /// </summary>
        /// <param name="gameObject"></param>
        private void StoreOriginal(GameObject gameObject)
        {
            if (!originalNailArts.Keys.Contains(gameObject.name))
            {
                originalNailArts.Add(gameObject.name, gameObject.transform.localScale);
            }
        }

        /// <summary>
        /// String representation of the scale as (x,y)
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        private string ToString(Vector3 scale)
        {
            return $"({scale.x},{scale.y})";
        }
    }
}