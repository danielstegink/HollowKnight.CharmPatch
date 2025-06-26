using Newtonsoft.Json.Linq;
using System;

namespace CharmPatch.OtherModHelpers
{
    public static class Exaltation
    {
        /// <summary>
        /// Gets an Exaltation setting via the save file
        /// </summary>
        /// <param name="saveIndex"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetProperty(int saveIndex, string propertyName)
        {
            try
            {
                // Get the mod data
                string modData = SaveFile.GetModdedSaveFile(saveIndex);

                // Convert the json string to a json object
                JObject saveFile = JObject.Parse(modData);
                if (saveFile == null)
                {
                    SharedData.Log("Save File JObject not found");
                    return "false";
                }

                JToken token = saveFile["modData"];
                if (token == null)
                {
                    SharedData.Log("modData JToken not found");
                    return "false";
                }

                token = token["Exaltation"];
                if (token == null)
                {
                    SharedData.Log("Exaltation JToken not found");
                    return "false";
                }

                token = token[propertyName];
                if (token == null)
                {
                    SharedData.Log($"{propertyName} JToken not found");
                    return "false";
                }

                return token.ToString();
            }
            catch (Exception ex) // If this breaks, we probly don't have the mod installed
            {
                SharedData.Log($"Exception while checking Exaltation: \n{ex.Message}\n{ex.StackTrace}");
                return "false";
            }
        }
    }
}
