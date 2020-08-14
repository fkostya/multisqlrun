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
        public static bool Offline { get; set; }

        static Config()
        {
            var jsonBytes = File.ReadAllBytes("config.json");
            var jsonDoc = JsonDocument.Parse(jsonBytes);

            var root = jsonDoc.RootElement;

            Url = getValue<string>(root, "url");
            ConnectionTemplate = getValue<string>(root, "connection_template");
            OfflineFilePath = getValue<string>(root, "offline_path");
            TimeputOpenConnection = getValue<int>(root, "timeputOpenConnection");
            SqlUname = getValue<string>(root, "sql_uname");
            SqlPwd = getValue<string>(root, "sql_pwd");
            Offline = getValue<bool>(root, "offline") || false;
        }

        static T getValue<T>(JsonElement element, string key)
            where T : IConvertible
        {
            T res = default(T);
            try
            {
                var tmp = element.GetProperty(key);

                if (typeof(T) == typeof(bool))
                    res = (T)(object)tmp.GetBoolean();
                else if (typeof(T) == typeof(int) || typeof(T) == typeof(Int32))
                    res = (T)(object)tmp.GetInt32();
                else if (typeof(T) == typeof(string))
                    res = (T)(object)tmp.GetString();
            }
            catch
            {
            }

            return res;
        }
    }
}