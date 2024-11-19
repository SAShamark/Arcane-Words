using System;
using System.IO;
using UnityEngine;

namespace Services.Storage
{
    public class StorageService : IStorageService
    {
        public void SaveData<T>(string key, T data)
        {
            try
            {
                string path = BuildPath(key);
                string json = JsonUtility.ToJson(data, prettyPrint: true);

                using (var fileStream = new StreamWriter(path))
                {
                    fileStream.Write(json);
                }

                Debug.Log($"Data successfully saved at [{path}].");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data with key [{key}]: {ex.Message}");
            }
        }

        public T LoadData<T>(string key, T defaultValue = default)
        {
            try
            {
                string path = BuildPath(key);

                if (File.Exists(path))
                {
                    using var fileStream = new StreamReader(path);
                    string json = fileStream.ReadToEnd();
                    var data = JsonUtility.FromJson<T>(json);
                    Debug.Log($"Data successfully loaded for key [{key}].");
                    return data;
                }

                Debug.LogWarning($"File not found for key [{key}]. Returning default value.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data with key [{key}]: {ex.Message}. Returning default value.");
            }

            return defaultValue;
        }

        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, $"{key}.json");
        }
    }
}