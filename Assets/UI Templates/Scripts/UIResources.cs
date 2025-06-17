namespace RemoteEducation.UI
{
    /// <summary>
    ///     Constants for loading COREv1 Legacy UI package assets from the Resources directory.
    /// </summary>
    public static class UIResources 
    {
        public static readonly string BaseResourcesFolder = "COREv1 UI/";


        // UI Template Resources

        /// <summary> Resources folder path containing Window template prefabs </summary>
        public static readonly string WindowTemplateResourcePath = BaseResourcesFolder + "Windows/";

        /// <summary> Resources folder path containing all other resource templates required for the package </summary>
        public static readonly string UITemplateResourcePath = BaseResourcesFolder + "Templates/";


        // Tooltip Resources

        /// <summary> Resource asset path for the prefab containing a canvas and the <see cref="TooltipController"/></summary>
        public static readonly string TooltipCanvasAssetPath = BaseResourcesFolder + "Tooltip/Tooltip Canvas";

        /// <summary> Resource asset path for default Tooltip style</summary>
        public static readonly string DefaultTooltipStyleResource = BaseResourcesFolder + "Tooltip/TooltipStyle";
    }
}