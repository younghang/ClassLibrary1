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
        public static void LoadConfigurations()
        {
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
            foreach (var item in settingItems)
            {
                CONFIGURE_TEXT += (item.Key + "=" + item.Value);
            }
            File.WriteAllText(ConfigureFileName, CONFIGURE_TEXT);
        }
        public static string GetSettingItem(string key)
        {
            if(settingItems.ContainsKey(key))
            {
                return settingItems[key];
            }
            return "";
        }
        public static void SetSettingItem(string key,string value)
        {
            if (key != "" && !settingItems.ContainsKey(key))
            {
                settingItems.Add(key, value);
            }
        }
    }
}
