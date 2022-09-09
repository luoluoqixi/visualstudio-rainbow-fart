using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace RainbowFart_VisualStudio
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RainbowFart
    {
        #region default
        private static RainbowFart _instance;
        /// <summary> 获取命令的实例。 </summary>
        public static RainbowFart Instance => _instance ?? (_instance = new RainbowFart());
        /// <summary> VS包提供这个命令，不为空。 </summary>
        private AsyncPackage package;
        /// <summary> 从所有者包中获取服务提供者 </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider { get { return this.package; } }
        private RainbowFart() { }
        #endregion

        #region init
        /// <summary> 初始化命令的单例实例 </summary>
        public async Task InitializeAsync(AsyncPackage package, Setting setting)
        {
            // 切换到主线程-在MainCommand的构造函数中调用AddCommand需要 the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            var commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Assumes.Present(commandService);
            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE2;
            Assumes.Present(dte);
            var events = dte.Events;
            _dte = dte;
            _events = events;
            this.setting = setting;
            this.package = package;

            setting.LoadSettingsFromStorage();

            InitEvent(commandService);
            InitApp();
        }
        private void InitApp()
        {
            appData = new AppData(setting);
        }
        private void InitEvent(OleMenuCommandService commandService)
        {
            var toggle = new OleMenuCommand(this.OnToggleClick, new CommandID(Consts.MenuGroup, Consts.Btn_Toggle));
            commandService.AddCommand(toggle);

            var openSetting = new OleMenuCommand(this.OnOpenSettingClick, new CommandID(Consts.MenuGroup, Consts.Btn_OpenSetting));
            commandService.AddCommand(openSetting);

            var testAudio = new MenuCommand(this.OnTestAudioClick, new CommandID(Consts.MenuGroup, Consts.Btn_Test));
            commandService.AddCommand(testAudio);

            var refresh = new MenuCommand(this.OnRefreshClick, new CommandID(Consts.MenuGroup, Consts.Btn_Refresh));
            commandService.AddCommand(refresh);

            var openAudioDir = new MenuCommand(this.OnOpenAudioDirClick, new CommandID(Consts.MenuGroup, Consts.Btn_OpenAudioDir));
            commandService.AddCommand(openAudioDir);

            var about = new MenuCommand(this.OnAboutClick, new CommandID(Consts.MenuGroup, Consts.Btn_About));
            commandService.AddCommand(about);
        }
        #endregion

        #region property
        public Setting setting;
        public DTE2 _dte;
        private Events _events;
        public AppData appData;
        #endregion

        #region Menu Event
        private void OnToggleClick(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            setting.EnableAudiopoint = !setting.EnableAudiopoint;
            setting.SaveSettingsToStorage();
            ShowDialog("提示", "visualstudio-rainbow-fart 已" + (setting.EnableAudiopoint ? "启动" : "关闭"));
        }
        private void OnOpenSettingClick(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            package.ShowOptionPage(typeof(Setting));
        }
        private void OnTestAudioClick(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string testAudio = Path.Combine(setting.RootPath, "Audio/piano.mp3");
            SoundUtility.Instance.Play(testAudio);
        }
        private void OnRefreshClick(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            InitApp();
        }
        private void OnOpenAudioDirClick(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string audioPath = setting.AudioPath;
            if (!Directory.Exists(audioPath))
                Directory.CreateDirectory(audioPath);
            Utility.OpenExplorer(audioPath);
        }
        private void OnAboutClick(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string version = getVsixVersion();
            ShowDialog("关于", "彩虹屁 (visualstudio-rainbow-fart) - v" + version);
        }
        private string getVsixVersion()
        {
            var asmDir = setting.RootPath;
            var manifestPath = Path.Combine(asmDir, "extension.vsixmanifest");
            var version = "?";
            if (File.Exists(manifestPath))
            {
                var doc = new System.Xml.XmlDocument();
                doc.Load(manifestPath);
                var metaData = doc.DocumentElement.ChildNodes.Cast<System.Xml.XmlElement>().First(x => x.Name == "Metadata");
                var identity = metaData.ChildNodes.Cast<System.Xml.XmlElement>().First(x => x.Name == "Identity");
                version = identity.GetAttribute("Version");
            }
            return version;
        }
        #endregion

        public static void ShowDialog(string title, string message)
        {
            VsShellUtilities.ShowMessageBox(
            Instance.package,
            message,
            title,
            OLEMSGICON.OLEMSGICON_INFO,
            OLEMSGBUTTON.OLEMSGBUTTON_OK,
            OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}