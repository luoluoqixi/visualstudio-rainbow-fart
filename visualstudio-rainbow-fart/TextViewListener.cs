#if DEBUG
#define OpenDebug  
#endif

using System;
using System.IO;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.Text;

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
            if (RainbowFart.Instance.setting != null)
                Timeout = TimeSpan.FromSeconds(RainbowFart.Instance.setting.TimeOutSeconds);
        }

        StringBuilder sb = new StringBuilder(128);
        private readonly Random random = new Random();
        static DateTime LastChangedTime = DateTime.MinValue;
        TimeSpan Timeout = TimeSpan.FromSeconds(10);
        TimeSpan Delay = TimeSpan.FromMilliseconds(500);
        private void TextBuffer_Changed(object sender, TextContentChangedEventArgs e)
        {
            if (e.Changes?.Count > 0)
            {
                foreach (var item in e.Changes)
                {
                    if (item == null) continue;

                    //item.NewText // 新文本
                    //item.OldText // 清除的文本
                    if (!string.IsNullOrWhiteSpace(item.OldText))
                    {
#if OpenDebug
                        System.Diagnostics.Debug.WriteLine($"Old:{item.NewText}  Pos:{item.OldPosition}");
#endif
                        if (sb.Length > 0)
                            sb.Remove(sb.Length - 1, 1);
                    }
                    if (!string.IsNullOrEmpty(item.NewText))
                    {
#if OpenDebug
                        System.Diagnostics.Debug.WriteLine($"New:{item.NewText}  Pos:{item.NewPosition}");
#endif
                        if (string.IsNullOrWhiteSpace(item.NewText) == false) // Not WhiteSpace
                            sb.Append(item.NewText.ToLower());

                        if (sb.Length > src.Consts.InputTextMaxLength)
                        {
                            sb.Remove(0, src.Consts.InputTextMaxLength);
                        }
                        if (DateTime.Now - LastChangedTime > Timeout)
                        {
                            if (KeywordsCheck(sb.ToString()))
                            {
                                sb.Clear();
                            }
                            else
                            {
                                if (sb.Length > 1)  // 删除以前的
                                    sb.Remove(0, sb.Length - 1);
                            }
                        }
                        else
                        {
                            if (KeywordsCheck(sb.ToString()))
                            {
                                sb.Clear();
                            }
                        }
                    }
                }
            }
        }
        private void TextBuffer_PostChanged(object sender, EventArgs e) { }
        private bool KeywordsCheck(string inputHistory)
        {
            LastChangedTime = DateTime.Now;
            if (RainbowFart.Instance.setting == null || !RainbowFart.Instance.setting.EnableAudiopoint) return false;
            var data = RainbowFart.Instance.data.Contributes;
            if (data == null || data.contributes == null) return false;

            foreach (var contribute in data.contributes)
            {
                if (contribute == null || contribute.keywords == null
                    || contribute.voices == null || contribute.voices.Length == 0) continue;

                foreach (var keyword in contribute.keywords)
                {
                    if (!inputHistory.Contains(keyword))
                        continue;

                    var voice = contribute.voices[random.Next(contribute.voices.Length)];
                    src.SoundUtility.Instance.PlayAbsolute(Path.Combine(RainbowFart.Instance.setting.RootPath, voice));
#if OpenDebug
                    System.Diagnostics.Debug.WriteLine($"inputHistory:{inputHistory} Match=>{keyword}");
#endif
                    return true;
                }
            }
            return false;
        }
    }
}