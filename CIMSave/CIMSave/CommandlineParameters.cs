using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
//using CIMCollect.Properties;
//using CIMSave.Properties;

namespace CIMSave
{
    // this probably violates a ton of best practices... oh well
    static class CommandlineParameters
    {
        static CommandlineParameters()
        {
            initialized = CommandlineSetup();
        }
        public static string[] Args;
        public static string _fileSaveFolder = string.Empty;
        public static Dictionary<string, string> KeyValues = new Dictionary<string, string>();
        public static bool initialized;

        public static bool CommandlineSetup()
        {
            var connections = System.Configuration.ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings connection in connections)
            {
                var connName = connection.Name;
                KeyValues.Add(connection.Name, connection.ConnectionString);
                if (connName.Contains("."))
                {
                    string[] parts = connection.Name.Split('.');
                    string lastWord = parts[parts.Length - 1];
                    KeyValues.Add(lastWord, connection.ConnectionString);
                }
            }
            System.Collections.Specialized.NameValueCollection nvc  = new NameValueCollection();
            nvc.Add(System.Configuration.ConfigurationManager.AppSettings);
            var settings = System.Configuration.ConfigurationManager.AppSettings;
            foreach(SettingsProperty setting in settings)
            {
                KeyValues.Add(setting.Name, setting.DefaultValue.ToString());
            }
            return true;
        }

        public static void Set(string[] args) //, Settings settings)
        {
            Args = args;
            foreach (var arg in args)
            {
                if (arg.StartsWith("-f")) _fileSaveFolder = arg.Remove(0, 2);
                if (arg.StartsWith("-") && arg.Length > 2) KeyValues.Add(arg.Substring(0, 2), arg.Remove(0, 2));
            }
            //foreach (SettingsProperty property in settings.Properties)
            //{
            //    KeyValues.Add(property.Name, property.DefaultValue.ToString());
            //}
        }

        public static bool TryGetValue(string key, out string result)
        {
            if (KeyValues.TryGetValue(key, out string value))
            {
                result = value;
                return true;
            }
            else
            {
                result = string.Empty;
                return false;
            }
        }

        public static string Value(string key, string defaultvalue)
        {
            if (KeyValues.TryGetValue(key, out string value))
            {
                return value;
            }
            else
            {
                return defaultvalue;
            }
        }

    }
}
