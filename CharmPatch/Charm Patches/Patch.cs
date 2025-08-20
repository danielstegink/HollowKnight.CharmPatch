namespace CharmPatch.Charm_Patches
{
    public interface Patch
    {
        /// <summary>
        /// If the patch is active or not
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Starts the patch
        /// </summary>
        public void Start();

        /// <summary>
        /// Stops the patch
        /// </summary>
        public void Stop();
    }
}