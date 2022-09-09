using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace RainbowFart_VisualStudio
{
    public class Setting : DialogPage
    {
        public Setting()
        {
            enableAudiopoint = true;
            rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            audioPath = Path.Combine(rootPath, "Audio/default");
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
        private string audioPath;
        [Category(Consts.OptionSubmenu)]
        [DisplayName(Consts.OptionsFolderText)]
        public string AudioPath
        {
            get
            {
                return audioPath;
            }
            set
            {
                audioPath = value;
            }
        }
        private string rootPath;
        public string RootPath
        {
            get
            {
                return rootPath;
            }
        }
    }
}
