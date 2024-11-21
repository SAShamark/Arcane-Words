using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace Game
{
    public class GameDataService
    {
        private LevelsData _levelsData;

        public List<string> Levels => _levelsData.Levels;
        public List<GameWord> AllGameWords => _levelsData.GameWords;
        public Dictionary<string, List<GameWord>> LevelsGameWords => _levelsData.LevelsGameWords;

        public void Initialize(LevelsConfig levelsConfig)
        {
            LevelsData parsedData = ParseLevelsDataFromJson(levelsConfig.LevelsFileData);
            _levelsData = new LevelsData(parsedData.Levels, parsedData.GameWords);
            _levelsData.InitializeLevelsGameWords(BuildWordsByLevel());
        }

        private LevelsData ParseLevelsDataFromJson(TextAsset levelsJsonFile)
        {
            var emptyLevelsData = new LevelsData();
            return LoadFromJson(levelsJsonFile, emptyLevelsData);
        }

        private Dictionary<string, List<GameWord>> BuildWordsByLevel()
        {
            var wordsGroupedByLevel = new Dictionary<string, List<GameWord>>();

            foreach (string levelName in _levelsData.Levels)
            {
                List<GameWord> validWordsForLevel = GetValidWordsForLevel(levelName);
                wordsGroupedByLevel.Add(levelName, validWordsForLevel);
            }

            return wordsGroupedByLevel;
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

        private List<GameWord> GetValidWordsForLevel(string levelName)
        {
            Dictionary<char, int> levelLetterCounts = CountLetters(levelName);
            var validWords = new List<GameWord>();
            var seenWords = new HashSet<string>();

            foreach (GameWord gameWord in _levelsData.GameWords)
            {
                if (CanBuildWordFromLetters(gameWord.Word, levelLetterCounts) && seenWords.Add(gameWord.Word))
                {
                    validWords.Add(gameWord);
                }
            }

            return validWords;
        }

        private Dictionary<char, int> CountLetters(string text)
        {
            var letterCounts = new Dictionary<char, int>();

            foreach (char letter in text)
            {
                if (!letterCounts.TryAdd(letter, 1))
                    letterCounts[letter]++;
            }

            return letterCounts;
        }

        private bool CanBuildWordFromLetters(string word, Dictionary<char, int> availableLetters)
        {
            Dictionary<char, int> wordLetterCounts = CountLetters(word);

            foreach (KeyValuePair<char, int> letter in wordLetterCounts)
            {
                if (!availableLetters.TryGetValue(letter.Key, out int availableCount) || availableCount < letter.Value)
                    return false;
            }

            return true;
        }
    }
}