namespace CharmPatch.Charm_Patches
{
    public interface Patch
    {
        /// <summary>
        /// If the patch is active or not
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Starts the patch. Called on mod initialization
        /// </summary>
        public void Start();
    }
}