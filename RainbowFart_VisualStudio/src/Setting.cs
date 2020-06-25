using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace RainbowFart_VisualStudio.src
{
    public class Setting : DialogPage
    {
        public Setting()
        {
            EnableAudiopoint = true;
            RootPath = SoundUtility.Instance.GetAudioFolderPath() + "/" + Consts.AudioDefaultFolder + "/";
        }
        private bool enableAudiopoint;
        [Category(Consts.OptionSubmenu)]
        [DisplayName(Consts.OptionsEnableAudioText)]
        [Description(Consts.OptionsEnableAudioDescription)]
        public bool EnableAudiopoint
        {
            get
            {
                return enableAudiopoint;
            }
            set
            {
                enableAudiopoint = value;
            }
        }
        private string rootPath;
        [Category(Consts.OptionSubmenu)]
        [DisplayName(Consts.OptionsFolderText)]
        [Description(Consts.OptionsFolderDescription)]
        public string RootPath
        {
            get
            {
                return rootPath;
            }
            set
            {
                rootPath = value;
            }
        }
    }
}
