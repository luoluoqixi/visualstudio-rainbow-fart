using System;

namespace RainbowFart_VisualStudio.src
{
    public static class Consts
    {
        /// <summary> MainCommandPackage GUID string. </summary>
        public const string PackageGuidString = "808b4f12-291b-462d-819d-e9f81f4f62c5";
        public const string Guid = "199d68e1-de36-4a7f-beb9-cacff086f594";
        public static readonly Guid MenuGroup = new Guid(Guid);
        public const int Btn_Enable = 0x0100;
        public const int Btn_Test = 0x0101;


        public const string Name = "彩虹屁";
        public const string OptionSubmenu = "General";
        public const string OptionsEnableAudioText = "Enable RainbowFart";
        public const string OptionsEnableAudioDescription = "Enable rainbowfart sound effect";

        public const string OptionsFolderText = "Root Path";
        public const string OptionsFolderDescription = "Root Path";

        public const string AudioContributesJson = "contributes.json";
        public const string AudioDefaultFolder = "default";
    }
}
