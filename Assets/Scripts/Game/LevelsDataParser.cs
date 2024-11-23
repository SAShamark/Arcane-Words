using UnityEngine;

namespace Game
{
    public class LevelsDataParser
    {
        public LevelsData ParseLevelsDataFromJson(TextAsset levelsJsonFile)
        {
            var emptyLevelsData = new LevelsData();
            return LoadFromJson(levelsJsonFile, emptyLevelsData);
        }

        private T LoadFromJson<T>(TextAsset jsonFile, T fallbackData)
        {
            if (jsonFile == null || string.IsNullOrWhiteSpace(jsonFile.text))
                return fallbackData;

            try
            {
                return JsonUtility.FromJson<T>(jsonFile.text);
            }
            catch
            {
                return fallbackData;
            }
        }
    }
}