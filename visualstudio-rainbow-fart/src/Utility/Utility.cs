using System.IO;

namespace RainbowFart_VisualStudio
{
    public static class Utility
    {
        public static void OpenExplorer(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            path = path.Replace('/', '\\');
            if (Directory.Exists(path))
            {
                System.Diagnostics.Process.Start("explorer.exe", "/open," + path);
                return;
            }
            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
                return;
            }
            string folderName = Path.GetDirectoryName(path);
            if (Directory.Exists(folderName))
                System.Diagnostics.Process.Start("explorer.exe", "/select," + folderName);
        }
    }
}
