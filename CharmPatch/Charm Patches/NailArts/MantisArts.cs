using CharmPatch.OtherModHelpers;
using Newtonsoft.Json.Linq;
using Satchel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharmPatch.Charm_Patches
{
    public class MantisArts : CharmPatch
    {
        public void AddHook()
        {
            On.HeroController.Update += Start;
        }

        /// <summary>
        /// Stores the original scales of the nail arts
        /// </summary>
        private Dictionary<string, Vector3> originalNailArts = new Dictionary<string, Vector3>();

        private void Start(On.HeroController.orig_Update orig, HeroController self)
        {
            // Get all nail art objects
            List<GameObject> gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>()
                                            .AsEnumerable()
                                            .Where(x => SharedData.nailArtNames.Contains(x.name))
                                            .ToList();

            foreach (GameObject gameObject in gameObjects)
            {
                // Store Nail Arts object so we can reference it at its original parameters
                // This makes it easier to change the size modifier if the player
                //   switches charms
                StoreOriginal(gameObject);

                // Get the modifier. If the patch is turned off, we want to use the 
                //   original scale
                float modifier = 1.0f;
                if (SharedData.globalSettings.mantisArtsOn)
                {
                    modifier = GetModifier();
                    //SharedData.Log($"Mantis Arts modifier: {modifier}");
                }

                // Apply the modifier to the Nail Art
                Vector3 currentScale = gameObject.transform.localScale;
                Vector3 originalScale = originalNailArts[gameObject.name];
                gameObject.SetScale(originalScale.x * modifier, originalScale.y * modifier);

                //Vector3 newScale = gameObject.transform.localScale;
                //SharedData.Log($"{gameObject} buffed: {ToString(currentScale)} -> {ToString(originalScale)} -> {ToString(newScale)}");
            }

            orig(self);
        }

        /// <summary>
        /// Gets how much to increase the range by
        /// </summary>
        /// <returns></returns>
        private float GetModifier()
        {
            // Longnail increases nail range by 15%
            if (PlayerData.instance.equippedCharm_18 &&
                !PlayerData.instance.equippedCharm_13)
            {
                float longnailModifier = 1.15f;
                if (SharedData.charmChangerInstalled)
                {
                    JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "longnailScale");
                    float charmChangerModifier = float.Parse(modifierToken.ToString()) / 100;
                    longnailModifier = 1 + charmChangerModifier;
                }

                return longnailModifier;
            }

            // Mark of Pride increases nail range by 25%
            if (!PlayerData.instance.equippedCharm_18 &&
                PlayerData.instance.equippedCharm_13)
            {
                float mopModifier = 1.25f;
                if (SharedData.charmChangerInstalled)
                {
                    JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "markOfPrideScale");
                    float charmChangerModifier = float.Parse(modifierToken.ToString()) / 100;
                    mopModifier = 1 + charmChangerModifier;
                }

                return mopModifier;
            }

            // The two stack on each other, so equipping both increases range by 40%
            if (PlayerData.instance.equippedCharm_18 &&
                PlayerData.instance.equippedCharm_13)
            {
                float bothModifier = 1.40f;
                if (SharedData.charmChangerInstalled)
                {
                    JToken modifierToken = CharmChanger.GetProperty(SharedData.currentSave, "longnailMarkOfPrideScale");
                    float charmChangerModifier = float.Parse(modifierToken.ToString()) / 100;
                    bothModifier = 1 + charmChangerModifier;
                }

                return bothModifier;
            }

            return 1.0f;
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