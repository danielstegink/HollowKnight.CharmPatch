using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharmPatch.OtherModHelpers
{
    public static class CharmChanger
    {
        /// <summary>
        /// Gets a Charm Changer setting via the save file
        /// </summary>
        /// <param name="saveIndex"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static JToken GetProperty(int saveIndex, string propertyName)
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
                    return null;
                }

                JToken token = saveFile["modData"];
                if (token == null)
                {
                    SharedData.Log("modData JToken not found");
                    return null;
                }

                token = token["CharmChanger"];
                if (token == null)
                {
                    SharedData.Log("CharmChanger JToken not found");
                    return null;
                }

                token = token[propertyName];
                if (token == null)
                {
                    SharedData.Log($"{propertyName} JToken not found");
                    return null;
                }

                return token;
            }
            catch (Exception ex) // If this breaks, we probly don't have Pale Court installed
            {
                SharedData.Log($"Exception while checking Charm Changer: \n{ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
    }
}
