using System;
using System.Collections.Generic;
using Services.Clock;
using UI.Screens.GameMenu;

namespace Game
{
    public sealed class GameController : IDisposable
    {
        private string _written;
        private string _levelWord;
        private GameMenuScreen _gameMenuScreen;
        private List<GameWord> _levelWords;

        private List<string> _unlockedWords = new();
        private readonly IClockService _clockService;
        private GameDataService _gameDataService;
        private float _elapsedTime;


        public GameController(IClockService clockService)
        {
            _clockService = clockService;
        }

        public void StartGame(GameMenuScreen gameMenuScreen, GameDataService gameDataService, string level)
        {
            _gameMenuScreen = gameMenuScreen;
            _gameDataService = gameDataService;
            _levelWord = level;
            _levelWords = _gameDataService.LevelsGameWords[level];
            LevelProgressData levelProgressData = _gameDataService.LevelsProgressData[level];
            _elapsedTime = levelProgressData.GameTime;

            if (_gameMenuScreen != null)
            {
                _gameMenuScreen.Init(level, _levelWords, levelProgressData.UnlockedWords, _elapsedTime);
                _unlockedWords = levelProgressData.UnlockedWords;
                _gameMenuScreen.UpdateShowedWordCount(levelProgressData.UnlockedWords.Count);
                Subscribe();
            }

            _clockService.StartStopwatch(ClockConstants.GAME_TIMER);
        }

        private void SaveAndResetGameData()
        {
            _written = string.Empty;
            float time = _elapsedTime + _clockService.StopStopwatch(ClockConstants.GAME_TIMER);
            var levelProgressData = new LevelProgressData(_levelWord, _unlockedWords, time);
            _gameDataService.UpdateLevelsProgressData(levelProgressData);

            _gameMenuScreen.ActivatePressedButtons();
            Unsubscribe();
        }

        private void Subscribe()
        {
            _gameMenuScreen.OnAddSign += AddSign;
            _gameMenuScreen.OnEraseSign += EraseSign;
            _gameMenuScreen.OnClearWord += ClearWord;
            _gameMenuScreen.OnExit += SaveAndResetGameData;
        }

        private void Unsubscribe()
        {
            _gameMenuScreen.Unsubscribe();
            _gameMenuScreen.OnAddSign -= AddSign;
            _gameMenuScreen.OnEraseSign -= EraseSign;
            _gameMenuScreen.OnClearWord -= ClearWord;
            _gameMenuScreen.OnExit -= SaveAndResetGameData;
            _gameMenuScreen.OnExit -= SaveAndResetGameData;
        }

        private void CheckWrittenWord()
        {
            _gameMenuScreen.ActivatePressedButtons();
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
            _clockService.StartTimer(ClockConstants.WORD_WRITTEN,
                _gameDataService.GamePlayConfig.SecondsToCheckWord, CheckWrittenWord);
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
                _clockService.StartTimer(ClockConstants.WORD_WRITTEN,
                    _gameDataService.GamePlayConfig.SecondsToCheckWord, CheckWrittenWord);
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
            Unsubscribe();
        }
    }
}