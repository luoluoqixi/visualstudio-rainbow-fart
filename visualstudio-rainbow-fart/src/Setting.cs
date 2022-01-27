using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;

namespace RainbowFart_VisualStudio.src
{
    public class Setting : DialogPage
    {
        public Setting()
        {
            EnableAudiopoint = true;
            RootPath = System.IO.Path.Combine(SoundUtility.Instance.GetAudioFolderPath(), Consts.AudioDefaultFolder);
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
        /// <summary>
        /// 钳位 value=[min,max]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="Max"></param>
        /// <returns></returns>
        public static T LimitCC<T>(T value, T min, T Max) where T : struct, IComparable<T>
        {
            // value>=min && value<=Max
            if (value.CompareTo(min) >= 0 && value.CompareTo(Max) <= 0) return value;
            if (value.CompareTo(min) < 0) return min; // value<min
            return Max;
        }
        
        private int _TimeOutSeconds = 10;

        [Category(Consts.OptionSubmenu)]
        [DisplayName(Consts.TimeOutSecondsText)]
        public int TimeOutSeconds
        {
            get { return _TimeOutSeconds; }
            set
            {
                _TimeOutSeconds = LimitCC(value, 5, 60);
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
