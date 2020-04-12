using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YHExcelAddin
{
    class FileSettings
    {
        private static Dictionary<string, string> settingItems = new Dictionary<string, string>();
        public static readonly string SettingFilePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private static string ConfigureFileName = SettingFilePath + "/set.in";
        private static string CONFIGURE_TEXT = "";
        private static void LoadConfigurations()
        {
            settingItems.Clear();
            CONFIGURE_TEXT = "";
            if (File.Exists(ConfigureFileName))
            {
                CONFIGURE_TEXT = File.ReadAllText(ConfigureFileName);
            }
            string[] setItems= CONFIGURE_TEXT.Split('\n');
            for(int i=0;i<setItems.Length;i++)
            {
                if (setItems[i].StartsWith("["))
                    continue;
                if(setItems[i].Contains("="))
                {
                    string key = setItems[i].Split('=')[0];
                    string value = setItems[i].Split('=')[1];
                    if(key!=""&&!settingItems.ContainsKey(key))
                    {
                        settingItems.Add(key, value);
                    }
                }
            }
        }
        public static void SaveConfigurations()
        {
            CONFIGURE_TEXT = "";
            foreach (var item in settingItems)
            {
                CONFIGURE_TEXT += (item.Key + "=" + item.Value+"\n");
            }
            File.WriteAllText(ConfigureFileName, CONFIGURE_TEXT);
        }
        public static string GetSettingItem(string key)
        {
            LoadConfigurations();
            if (settingItems.ContainsKey(key))
            {
                return settingItems[key];
            }
            return "";
        }
        public static void SetSettingItem(string key,string value)
        {
            LoadConfigurations();
            if (key != "" && !settingItems.ContainsKey(key))
            {
                settingItems.Add(key, value);
            } 
            if(settingItems.ContainsKey(key))
            {
                settingItems[key] = value;
            }
            SaveConfigurations();
        }
    }
}
