using System;
using System.Collections.Generic;
using Game.Data;
using Services.Clock;
using Services.Currencies;
using UI.Core;
using UI.Screens.GameMenu;

namespace Game.GamePlay
{
    public sealed class GameController : IDisposable
    {
        private string _levelWord;
        private float _elapsedTime;
        private GameMenuScreen _gameMenuScreen;
        private GameDataManager _gameDataManager;
        private List<GameWord> _levelWords;
        private List<string> _unlockedWords = new();

        private HintManager _hintManager;
        private GameWordBuilder _gameWordBuilder;

        private readonly IClockService _clockService;
        private readonly CurrencyService _currencyService;
        private readonly IUIManager _uiManager;

        public GameController(IClockService clockService, CurrencyService currencyService, IUIManager uiManager)
        {
            _clockService = clockService;
            _currencyService = currencyService;
            _uiManager = uiManager;
        }

        public void StartGame(GameMenuScreen gameMenuScreen, GameDataManager gameDataManager, string level)
        {
            _gameMenuScreen = gameMenuScreen;
            _gameDataManager = gameDataManager;
            _levelWord = level;
            _levelWords = _gameDataManager.LevelsGameWords[level];

            LevelProgressData levelProgressData = _gameDataManager.LevelsProgressData[level];
            _elapsedTime = levelProgressData.GameTime;
            _unlockedWords = levelProgressData.UnlockedWords;

            InitGameWordBuilder();
            InitHintManager(levelProgressData);
            InitializeGameScreen(levelProgressData);
            Subscribe();
            _clockService.StartStopwatch(ClockConstants.GAME_TIMER);
        }

        private void InitHintManager(LevelProgressData levelProgressData)
        {
            _hintManager = new HintManager(_uiManager, _gameDataManager, _gameMenuScreen.WordInstances, _levelWords,
                _unlockedWords, _gameMenuScreen)
            {
                WordsWithHint = levelProgressData.WordsWithHint
            };
            _hintManager.UpdateHintButtonActivity();
        }

        private void InitGameWordBuilder()
        {
            _gameWordBuilder = new GameWordBuilder(_currencyService, _clockService, _gameDataManager, _gameMenuScreen,
                _unlockedWords, _levelWords);
        }

        private void InitializeGameScreen(LevelProgressData levelProgressData)
        {
            if (_gameMenuScreen != null)
            {
                int hintCount = _currencyService.GetCurrencyByType(CurrencyType.Hint).Currency;

                _gameMenuScreen.Init(_levelWord, _levelWords, _unlockedWords,
                    levelProgressData.WordsWithHint, _elapsedTime, hintCount);
                _gameMenuScreen.UpdateShowedWordCount(levelProgressData.UnlockedWords.Count);
            }
        }

        private void SaveAndResetGameData()
        {
            _gameWordBuilder.ClearWord();
            float time = _elapsedTime + _clockService.StopStopwatch(ClockConstants.GAME_TIMER);
            var levelProgressData = new LevelProgressData(_levelWord, _unlockedWords, _hintManager.WordsWithHint, time);
            _gameDataManager.UpdateLevelsProgressData(levelProgressData);
            _gameMenuScreen.ActivatePressedButtons();
            Unsubscribe();
        }

        private void Subscribe()
        {
            _gameMenuScreen.OnExit += SaveAndResetGameData;
            _gameWordBuilder.Subscribe();
            _hintManager.Subscribe();
        }

        private void Unsubscribe()
        {
            _gameMenuScreen.OnExit -= SaveAndResetGameData;
            _gameWordBuilder.Unsubscribe();
            _hintManager.Unsubscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}