using DanielSteginkUtils.Utilities;
using Modding;

namespace CharmPatch.OtherModHelpers
{
    public static class CharmChanger
    {
        /// <summary>
        /// Gets a setting from Charm Changer
        /// </summary>
        /// <param name="saveIndex"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static O GetProperty<O>(string propertyName)
        {
            object saveSettings = ClassIntegrations.GetProperty<IMod, object>(SharedData.charmChangerMod, "LS");
            return ClassIntegrations.GetField<object, O>(saveSettings, propertyName);
        }
    }
}