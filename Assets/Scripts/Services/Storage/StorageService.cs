using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.Storage
{
    public class StorageService : IStorageService
    {
        public void SaveData<T>(string key, T data)
        {
            string path = BuildPath(key);
            string json = JsonConvert.SerializeObject(data);
            using var fileStream = new StreamWriter(path);
            fileStream.Write(json);
        }

        public T LoadData<T>(string key, T defaultValue)
        {
            string path = BuildPath(key);

            if (File.Exists(path))
            {
                using var fileStream = new StreamReader(path);
                string json = fileStream.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }

            Debug.Log($"Returned default data by key [{key}] by value [{defaultValue}]");
            return defaultValue;
        }

        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key);
        }
    }
}