using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;

namespace RainbowFart_VisualStudio
{
    public class AppData
    {
        private Setting setting;
        private Contributes contributes;
        public Contributes Contributes
        {
            get { return contributes; }
        }
        public AppData(Setting setting)
        {
            this.setting = setting;
            InitAudioData();
        }
        private void InitAudioData()
        {
            string contributesPath = Path.Combine(setting.AudioPath, Consts.AudioContributesJson);
            try
            {
                contributes = ParseContributesData(contributesPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        private static Contributes ParseContributesData(string path)
        {
            var result = new Contributes();
            if (!File.Exists(path))
                return result;
            string content = File.ReadAllText(path);
            JSONNode rootNode = JSON.Parse(content);
            if (!rootNode.ContainsKey("contributes"))
                return result;
            JSONNode contributes = rootNode["contributes"];
            JSONArray arr = contributes.AsArray;

            var contributesList = new List<Contribute>();
            int count = arr.Count;
            for (int i = 0; i < count; i++)
            {
                var contributeData = contributes[i];
                if (contributeData == null) continue;
                if (!contributeData.ContainsKey("keywords") || !contributeData.ContainsKey("voices"))
                    continue;
                var keywords = contributeData["keywords"].AsArray;
                var voices = contributeData["voices"].AsArray;

                var keywordsList = new List<string>();
                var voicesList = new List<string>();

                for (int index = 0; index < keywords.Count; index++)
                {
                    string key = keywords[index].AsString;
                    if (!string.IsNullOrEmpty(key))
                        keywordsList.Add(key);
                }
                for (int index = 0; index < voices.Count; index++)
                {
                    string voice = voices[index].AsString;
                    if (!string.IsNullOrEmpty(voice))
                        voicesList.Add(voice);
                }
                var contribute = new Contribute();
                contribute.keywords = keywordsList.ToArray();
                contribute.voices = voicesList.ToArray();
                contributesList.Add(contribute);
            }
            result.contributes = contributesList.ToArray();
            return result;
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
