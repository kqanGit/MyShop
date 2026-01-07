using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MyShop_Frontend.Helpers
{
    public static class LocalSettingsHelper
    {
        private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyShop_Frontend");
        private static readonly string FilePath = Path.Combine(FolderPath, "settings.json");
        private static Dictionary<string, object> _settings;

        static LocalSettingsHelper()
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            if (File.Exists(FilePath))
            {
                try
                {
                    var json = File.ReadAllText(FilePath);
                    _settings = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
                }
                catch
                {
                    _settings = new Dictionary<string, object>();
                }
            }
            else
            {
                _settings = new Dictionary<string, object>();
            }
        }

        public static void Save()
        {
            try
            {
                var json = JsonSerializer.Serialize(_settings);
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }

        public static T? GetValue<T>(string key)
        {
            if (_settings.TryGetValue(key, out var val))
            {
                if (val is JsonElement element)
                {
                    try { return element.Deserialize<T>(); } catch { }
                }
                if (val is T tVal) return tVal;
            }
            return default;
        }

        public static void SetValue<T>(string key, T value)
        {
            _settings[key] = value;
            Save();
        }

        public static void Remove(string key)
        {
            if (_settings.Remove(key))
            {
                Save();
            }
        }
    }
}
