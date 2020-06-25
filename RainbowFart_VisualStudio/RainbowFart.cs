using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using RainbowFart_VisualStudio.src;
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

            var opemSetting = new OleMenuCommand(this.OpenSetting, new CommandID(Consts.MenuGroup, Consts.Btn_Enable));
            commandService.AddCommand(opemSetting);

            var testAudio = new MenuCommand(this.TestAudio, new CommandID(Consts.MenuGroup, Consts.Btn_Test));
            commandService.AddCommand(testAudio);

            SoundUtility.Instance.Init();
            data = new Data(setting.RootPath);

            //events.TextEditorEvents.LineChanged += OnLineChanged;
        }
        #endregion

        #region property
        public Setting setting;
        public DTE2 _dte;
        private Events _events;
        public Data data;
        #endregion

        private void OpenSetting(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            package.ShowOptionPage(typeof(Setting));
            //setting.EnableAudiopoint = !setting.EnableAudiopoint;
            //setting.SaveSettingsToStorage();
            //ShowDialog("提示", "RainbowFart 成功" + (setting.EnableAudiopoint ? "启动" : "关闭"));
        }
        private void TestAudio(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            SoundUtility.Instance.Play("piano.mp3");
        }
        public string GetAudioFolderPath() { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Audio"); }
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