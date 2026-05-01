namespace PropReskinner
{
    public enum PropReskinnerStyles
    {
        Default = 0,
        PreCrashNomai,
    }

    public enum PaintedDetailsMode
    {
        AltMaterial,
        Faded,
        Removed,
        // Restored,
    }

    public class PropReskinnerInfo
    {
        /// <summary>
        /// Determines what materials & textures the props get reskinned to.
        /// </summary>
        public PropReskinnerStyles style;

        /// <summary>
        /// How are Nomai painted detail textures handled?
        /// `Faded` : keeps original paint details.
        /// `Removed` : washes the paint off.
        /// `AltMaterial` : replaces painted surfaces with detailed metal, similar to the props from Mod Jam 3 & 5's Starship Community and Central Station.
        /// </summary>
        public PaintedDetailsMode paintedDetails;

        /// <summary>
        /// Path to diffuse texture to use for suited Nomai charaters (e.g. Solanum, corpses)
        /// </summary>
        public string nomaiCharacterSuit;

        public string nomaiMaskTrim;

        /// <summary>
        /// Paths to props that you want to get reskinned. Will reskin any children as well.
        /// </summary>
        public string[] props;
    }
}
