using System.Collections.Generic;
using System.Linq;
using Services.Storage;
using UI.Core.Data;
using UnityEngine;

namespace Game.Data
{
    public class GameDataManager
    {
        private LevelsData _levelsData;
        private LevelsDataParser _levelsDataParser;
        private WordsBuilder _wordsBuilder;

        private readonly IStorageService _storageService;
        private readonly LevelsConfig _levelsConfig;

        public GamePlayConfig GamePlayConfig { get; private set; }

        public List<string> Levels => _levelsData.Levels;
        public Dictionary<string, LevelProgressData> LevelsProgressData { get; private set; } = new();
        public List<GameWord> AllGameWords => _levelsData.GameWords;
        public Dictionary<string, List<GameWord>> LevelsGameWords => _levelsData.LevelsGameWords;

        public GameDataManager(GamePlayConfig gamePlayConfig, LevelsConfig levelsConfig, IStorageService storageService)
        {
            GamePlayConfig = gamePlayConfig;
            _storageService = storageService;
            _levelsConfig = levelsConfig;
        }

        public void Initialize()
        {
            _levelsDataParser = new LevelsDataParser();

            LevelsData parsedData = _levelsDataParser.ParseLevelsDataFromJson(_levelsConfig.LevelsFileData);
            _levelsData = new LevelsData(parsedData.Levels, parsedData.GameWords);

            _wordsBuilder = new WordsBuilder(_levelsData.GameWords);
            Dictionary<string, List<GameWord>> wordsByLevel = _wordsBuilder.BuildWordsByLevel(_levelsData.Levels);
            _levelsData.InitLevelsGameWords(wordsByLevel);

            InitLevelsProgressData();
        }

        private void InitLevelsProgressData()
        {
            foreach (string level in _levelsData.Levels.Where(level => !LevelsProgressData.ContainsKey(level)))
            {
                var levelProgressData = new LevelProgressData(level, new List<string>(), new List<string>());
                LevelsProgressData[level] = levelProgressData;
            }

            LevelsProgressData = _storageService.LoadData(StorageConstants.GAME_PROGRESS, LevelsProgressData);
        }

        public void UpdateLevelsProgressData(LevelProgressData levelProgress)
        {
            LevelsProgressData[levelProgress.Word] = levelProgress;
            _storageService.SaveData(StorageConstants.GAME_PROGRESS, LevelsProgressData);
        }

        public int CalculateLevelProgress(string level)
        {
            float totalWords = LevelsGameWords[level].Count;
            float unlockedWords = LevelsProgressData[level].UnlockedWords.Count;

            if (totalWords == 0)
            {
                Debug.LogError($"Level {level} has no words.");
                return 0;
            }

            return Mathf.RoundToInt(unlockedWords / totalWords * 100);
        }
    }
}