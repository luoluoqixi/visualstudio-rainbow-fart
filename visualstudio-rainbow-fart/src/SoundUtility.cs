using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace RainbowFart_VisualStudio.src
{
    public class SoundUtility
    {
        private static SoundUtility _instance;
        private static MediaPlayer _player = new MediaPlayer();
        public static SoundUtility Instance => _instance ?? (_instance = new SoundUtility());
        private SoundUtility() {  /* Init(); */ }
        /// <summary> 初始化 </summary>
        public void Init()
        {
            //_player.MediaEnded += (s, e) => _player.Close();
        }
        public void Play(string audioName)
        {
            var path = GetAudioPath(audioName);
            PlayAbsolute(path);
        }
        public void PlayAbsolute(string path)
        {
            _player.Stop();
            if (RainbowFart.Instance.setting == null) return;
            if (RainbowFart.Instance.setting.EnableAudiopoint)
            {
                if (!File.Exists(path)) return;
                _player.Open(new Uri(path, UriKind.Absolute));
                _player.Play();
            }
        }
        public void Stop()
        {
            _player.Stop();
        }
        public string GetAudioPath(string name)
        {
            return Path.Combine(GetAudioFolderPath(), name);
        }
        public virtual string GetAudioFolderPath() { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Audio"); }
    }
}