using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace RainbowFart_VisualStudio
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class TextViewListener : IWpfTextViewCreationListener
    {
        [Import]
        internal IEditorFormatMapService FormatMapService = null;
        public void TextViewCreated(IWpfTextView textView)
        {
            textView.TextBuffer.Changed += TextBuffer_Changed;
            textView.TextBuffer.PostChanged += TextBuffer_PostChanged;
        }
        private string inputHistory = "";
        private readonly Random random = new Random();
        private void TextBuffer_Changed(object sender, TextContentChangedEventArgs e)
        {
            //ThreadHelper.ThrowIfNotOnUIThread();
            if (e.Changes?.Count > 0)
            {
                foreach (var item in e.Changes)
                {
                    if (item == null) continue;
                    //item.NewText // 新文本
                    //item.OldText // 清除的文本
                    if (!string.IsNullOrEmpty(item.OldText))
                    {
                        //System.Windows.Forms.MessageBox.Show("Old:" + item.OldText + " Pos:" + item.OldPosition);
                    }
                    if (!string.IsNullOrEmpty(item.NewText))
                    {
                        //System.Windows.Forms.MessageBox.Show("New:" + item.NewText + " Pos:" + item.NewPosition);
                        var changeText = item.NewText;
                        inputHistory += changeText;
                        inputHistory = System.Text.RegularExpressions.Regex.Replace(inputHistory, @"\s", "");
                        if (inputHistory.Length > 100) inputHistory = inputHistory.Substring(inputHistory.Length - 101);
                        KeywordsCheck();
                    }
                }
            }
        }
        private void TextBuffer_PostChanged(object sender, EventArgs e) { }
        private void KeywordsCheck()
        {
            //System.Windows.Forms.MessageBox.Show("检测内容：" + data.Contributes.contributes[0].keywords[0]);
            if (RainbowFart.Instance.setting == null || !RainbowFart.Instance.setting.EnableAudiopoint) return;

            var data = RainbowFart.Instance.data.Contributes;

            if (data == null || data.contributes == null) return;

            foreach (var contribute in data.contributes)
            {
                if (contribute == null || contribute.keywords == null || contribute.voices == null || contribute.voices.Length == 0) continue;

                foreach (var keyword in contribute.keywords)
                {
                    if (!inputHistory.Contains(keyword)) continue;
                    inputHistory = "";
                    var voice = contribute.voices[random.Next(contribute.voices.Length)];
                    string path = RainbowFart.Instance.setting.RootPath + "/" + voice;
                    src.SoundUtility.Instance.PlayAbsolute(path);
                    //System.Windows.Forms.MessageBox.Show("成功匹配：" + keyword);
                    return;
                }
            }
        }
    }
}