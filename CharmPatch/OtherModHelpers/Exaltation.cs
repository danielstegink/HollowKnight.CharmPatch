using DanielSteginkUtils.Utilities;
using Modding;

namespace CharmPatch.OtherModHelpers
{
    public static class Exaltation
    {
        /// <summary>
        /// Gets a field from the Exaltation
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static O GetField<O>(string propertyName)
        {
            object saveSettings = ClassIntegrations.GetField<IMod, object>(SharedData.exaltationMod, "Settings");
            return ClassIntegrations.GetField<object, O>(saveSettings, propertyName);
        }
    }
}
