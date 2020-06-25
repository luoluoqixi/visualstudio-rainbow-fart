using System;
using System.IO;

namespace RainbowFart_VisualStudio.src
{
    public class Data
    {
        public Data(string folderPath)
        {
            rootPath = folderPath;
            var path = rootPath + "/" + Consts.AudioContributesJson;
            string data = File.ReadAllText(path);
            contributes = JsonTools.JsonToObject<Contributes>(data);
        }
        private string rootPath;
        private Contributes contributes;
        public Contributes Contributes
        {
            get { return contributes; }
        }
    }
    [Serializable]
    public class Contributes
    {
        public Contribute[] contributes;
    }
    [Serializable]
    public class Contribute
    {
        public string[] keywords;
        public string[] voices;
    }
}
