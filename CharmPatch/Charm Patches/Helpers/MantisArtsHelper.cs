using DanielSteginkUtils.Helpers.Attributes;

namespace CharmPatch.Charm_Patches.Helpers
{
    /// <summary>
    /// Helper class for Mantis Arts
    /// </summary>
    public class MantisArtsHelper : NailArtRangeHelper
    {
        public MantisArtsHelper(float modifier, bool performLogging = false) :
            base(CharmPatch.Instance.Name, "Mantis Arts", modifier, performLogging) { }
    }
}