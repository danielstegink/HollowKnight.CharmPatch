using Modding;

namespace CharmPatch.Charm_Patches
{
    public class JonisKindness : CharmPatch
    {
        public void AddHook()
        {
            ModHooks.CharmUpdateHook += Start;
        }

        /// <summary>
        /// Joni's Blessing gives 2 additional Masks
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controller"></param>
        private void Start(PlayerData data, HeroController controller)
        {
            if (SharedData.globalSettings.jonisKindnessOn &&
                PlayerData.instance.equippedCharm_27)
            {
                PlayerData.instance.joniHealthBlue += 2;
                PlayerData.instance.MaxHealth();
                //SharedData.Log($"Joni count: {PlayerData.instance.joniHealthBlue}");
            }
        }
    }
}