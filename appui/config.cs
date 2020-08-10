using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace appui
{
    static class Config
    {
        public static string Url { get; private set; }
        public static string ConnectionTemplate { get; private set; }
        public static string OfflineFilePath { get; private set; }
        public static int TimeputOpenConnection { get; set; }
        public static string SqlUname { get; set; }
        public static string SqlPwd { get; set; }


        static Config()
        {
            var jsonBytes = File.ReadAllBytes("config.json");
            var jsonDoc = JsonDocument.Parse(jsonBytes);

            var root = jsonDoc.RootElement;

            Url = geString(root, "url");
            ConnectionTemplate = geString(root, "connection_template");
            OfflineFilePath = geString(root, "offline_path");
            TimeputOpenConnection = getInt32(root, "timeputOpenConnection");
            SqlUname = geString(root, "sql_uname");
            SqlPwd = geString(root, "sql_pwd");
        }

        static int getInt32(JsonElement element, string key)
        {
            int res = 100;
            try
            {
                res = element.GetProperty(key).GetInt32();
            }
            catch { }

            return res;
        }

        static string geString(JsonElement element, string key)
        {
            string res = "";
            try
            {
                res = element.GetProperty(key).GetString();
            }
            catch { }

            return res;
        }
    }
}
