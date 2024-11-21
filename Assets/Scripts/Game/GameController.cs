using System;
using System.Collections.Generic;
using Services.Clock;
using Services.Storage;
using UI.Screens.GameMenu;

namespace Game
{
    public sealed class GameController : IDisposable
    {
        private float _secondsToCheckWord = 1.5f;
        private string _written;
        private string _levelWord;
        private GameMenuScreen _gameMenuScreen;
        private List<GameWord> _levelWords;

        private readonly List<string> _unlockedWords = new();
        private readonly IClockService _clockService;
        private readonly IStorageService _storageService;
        private GameDataService _gameDataService;
        private Dictionary<string, LevelData> _levelsData = new();

        public GameController(IClockService clockService, IStorageService storageService)
        {
            _clockService = clockService;
            _storageService = storageService;
        }

        public void StartGame(GameMenuScreen gameMenuScreen, GameDataService gameDataService, string level)
        {
            _gameMenuScreen = gameMenuScreen;
            _gameDataService = gameDataService;
            _levelWord = level;
            _levelWords = _gameDataService.LevelsGameWords[level];
            var levelData = new LevelData(string.Empty, new List<string>(), 0);
            if (_levelsData.ContainsKey(level))
            {
                _levelsData[level] = levelData;
            }

            _levelsData = _storageService.LoadData(StorageConstants.GAME_PROGRESS, _levelsData);

            if (_gameMenuScreen != null)
            {
                _gameMenuScreen.Init(level, _gameDataService.LevelsGameWords[level]);
                _gameMenuScreen.UpdateShowedWordCount(0);
                _gameMenuScreen.OnAddSign += AddSign;
                _gameMenuScreen.OnEraseSign += EraseSign;
                _gameMenuScreen.OnClearWord += ClearWord;
                _gameMenuScreen.OnExit += SaveAndResetGameData;
            }

            _clockService.StartStopwatch(ClockConstants.GAME_TIMER);
        }

        private void SaveAndResetGameData()
        {
            _written = string.Empty;
            float time = _clockService.StopStopwatch(ClockConstants.GAME_TIMER);
            _levelsData[_levelWord] = new LevelData(_levelWord, _unlockedWords, time);
            _storageService.SaveData(StorageConstants.GAME_PROGRESS, _levelsData);
            _gameMenuScreen.OnAddSign -= AddSign;
            _gameMenuScreen.OnEraseSign -= EraseSign;
            _gameMenuScreen.OnClearWord -= ClearWord;
            _gameMenuScreen.OnExit -= SaveAndResetGameData;
        }

        private void CheckWrittenWord()
        {
            for (var index = 0; index < _levelWords.Count; index++)
            {
                GameWord word = _levelWords[index];
                if (word.Word == _written && !_gameMenuScreen.WordInstances[index].IsUnlocked)
                {
                    _unlockedWords.Add(word.Word);
                    _gameMenuScreen.WordInstances[index].UnlockWord();
                    _gameMenuScreen.UpdateShowedWordCount(_unlockedWords.Count);
                    ClearWord();
                    return;
                }
            }

            ClearWord();
        }

        private void AddSign(char sign)
        {
            _clockService.StartTimer(ClockConstants.WORD_WRITTEN, _secondsToCheckWord, CheckWrittenWord);
            if (sign != ' ')
            {
                _written += sign;
            }

            _gameMenuScreen.UpdateWrittenText(_written);
        }

        private void EraseSign()
        {
            if (!string.IsNullOrEmpty(_written))
            {
                _written = _written.Substring(0, _written.Length - 1);
                _gameMenuScreen.UpdateWrittenText(_written);
                _clockService.StartTimer(ClockConstants.WORD_WRITTEN, _secondsToCheckWord, CheckWrittenWord);
            }
            else
            {
                _clockService.StopTimer(ClockConstants.WORD_WRITTEN);
            }
        }

        private void ClearWord()
        {
            _written = string.Empty;
            _gameMenuScreen.UpdateWrittenText(_written);
            _clockService.StopTimer(ClockConstants.WORD_WRITTEN);
        }

        public void Dispose()
        {
            _gameMenuScreen.OnAddSign -= AddSign;
            _gameMenuScreen.OnEraseSign -= EraseSign;
            _gameMenuScreen.OnClearWord -= ClearWord;
        }
    }

    public class LevelData
    {
        public string Word;
        public float GameTime;
        public List<string> UnlockedWords;

        public LevelData(string word, List<string> unlockedWords, float gameTime)
        {
            Word = word;
            GameTime = gameTime;
            UnlockedWords = unlockedWords;
        }
    }
}