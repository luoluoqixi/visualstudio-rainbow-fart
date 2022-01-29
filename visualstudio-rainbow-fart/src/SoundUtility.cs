using NAudio.Wave;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RainbowFart_VisualStudio.src
{
    public class SoundUtility : IDisposable
    {
        private static SoundUtility _instance;
        private WaveOutEvent outputDevice = new WaveOutEvent();
        private static AudioFileReader audioFile;
        public static SoundUtility Instance => _instance ?? (_instance = new SoundUtility());
        private SoundUtility()
        {
            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
        }

        private void OutputDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            audioFile?.Dispose();
            audioFile = null;
        }

        /// <summary> 
        /// 初始化 
        /// </summary>
        public void Init()
        {

        }
        public void Play(string audioName)
        {
            var path = GetAudioPath(audioName);
            PlayAbsolute(path);
        }
        public void PlayAbsolute(string path)
        {
            if (outputDevice.PlaybackState == PlaybackState.Playing) return;
            outputDevice.Stop();
     
            if (RainbowFart.Instance.setting == null) return;
            if (RainbowFart.Instance.setting.EnableAudiopoint)
            {
                if (!File.Exists(path)) return;
                Task t = new Task(() =>
                {
                    audioFile = new AudioFileReader(path);
                    outputDevice.Init(audioFile);
                    outputDevice.Volume = 1;
                    outputDevice.Play();

                });
                t.Start();
            }
        }
        public void Stop()
        {
            outputDevice.Stop();
        }
        public string GetAudioPath(string name)
        {
            return Path.Combine(GetAudioFolderPath(), name);
        }
        public virtual string GetAudioFolderPath() { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Audio"); }

        #region "IDisposable及析构实现"
        private bool isDisposed;
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            //清理托管资源
            if (disposing)
            {
                outputDevice?.Dispose();
                audioFile.Dispose();
            }

            //清理非托管资源

            //告诉自己已经被释放
            isDisposed = true;
        }
        ~SoundUtility()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}