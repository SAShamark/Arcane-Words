using System;
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
            LevelsData levelsData = ParseJsonFile(levelsConfig.LevelsFileData);
            _levelsData = new LevelsData(levelsData.Levels, levelsData.GameWords);
            _levelsData.InitializeLevelsGameWords(FilterLevelsGameWords());
        }

        private LevelsData ParseJsonFile(TextAsset levelsFileData)
        {
            var defaultData = new LevelsData(new List<string>(), new List<GameWord>());
            LevelsData loadedData = LoadDataFromTextAsset(levelsFileData, defaultData);
            return loadedData;
        }

        private Dictionary<string, List<GameWord>> FilterLevelsGameWords()
        {
            var levelsGameWords = new Dictionary<string, List<GameWord>>();
            foreach (string level in _levelsData.Levels)
            {
                List<GameWord> gameWords = FilterWords(level, _levelsData.GameWords);
                levelsGameWords.Add(level, gameWords);
            }

            return levelsGameWords;
        }

        private T LoadDataFromTextAsset<T>(TextAsset textAsset, T defaultValue)
        {
            if (textAsset == null || string.IsNullOrWhiteSpace(textAsset.text))
            {
                return defaultValue;
            }

            try
            {
                return JsonUtility.FromJson<T>(textAsset.text);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        private List<GameWord> FilterWords(string levelWord, List<GameWord> words)
        {
            Dictionary<char, int> levelWordCounts = CountLetters(levelWord);
            var validWords = new List<GameWord>();

            foreach (GameWord word in words)
            {
                Dictionary<char, int> wordCounts = CountLetters(word.Word);

                if (CanFormWord(wordCounts, levelWordCounts))
                {
                    validWords.Add(word);
                }
            }

            return validWords;
        }

        private Dictionary<char, int> CountLetters(string word)
        {
            var letterCounts = new Dictionary<char, int>();

            foreach (char letter in word)
            {
                if (!letterCounts.TryAdd(letter, 1))
                {
                    letterCounts[letter]++;
                }
            }

            return letterCounts;
        }

        private bool CanFormWord(Dictionary<char, int> wordCounts, Dictionary<char, int> levelWordCounts)
        {
            foreach (KeyValuePair<char, int> kvp in wordCounts)
            {
                if (!levelWordCounts.ContainsKey(kvp.Key) || levelWordCounts[kvp.Key] < kvp.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}