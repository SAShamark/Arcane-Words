using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Services.Clock;
using Services.Currencies;
using UI.Core;
using UI.Popups.Core;
using UI.Popups.Hint;
using UI.Screens.GameMenu;

namespace Game
{
    public sealed class GameController : IDisposable
    {
        private string _written;
        private string _levelWord;
        private float _elapsedTime;
        private GameMenuScreen _gameMenuScreen;
        private GameDataManager _gameDataManager;
        private HintPopup _hintPopup;
        private List<GameWord> _levelWords;
        private List<string> _unlockedWords = new();
        private List<string> _wordsWithHint = new();

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
            _wordsWithHint = levelProgressData.WordsWithHint;

            InitializeGameScreen(levelProgressData);

            Subscribe();

            _clockService.StartStopwatch(ClockConstants.GAME_TIMER);
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

        public void SaveAndResetGameData()
        {
            _written = string.Empty;
            float time = _elapsedTime + _clockService.StopStopwatch(ClockConstants.GAME_TIMER);
            var levelProgressData = new LevelProgressData(_levelWord, _unlockedWords, _wordsWithHint, time);
            _gameDataManager.UpdateLevelsProgressData(levelProgressData);
            _gameMenuScreen.ActivatePressedButtons();
            Unsubscribe();
        }

        private void Subscribe()
        {
            _gameMenuScreen.OnAddSign += AddSign;
            _gameMenuScreen.OnEraseSign += EraseSign;
            _gameMenuScreen.OnClearWord += ClearWord;
            _gameMenuScreen.OnHint += HintClicked;
        }

        private void Unsubscribe()
        {
            _gameMenuScreen.OnAddSign -= AddSign;
            _gameMenuScreen.OnEraseSign -= EraseSign;
            _gameMenuScreen.OnClearWord -= ClearWord;
            _gameMenuScreen.OnHint -= HintClicked;
            if (_hintPopup != null)
            {
                _hintPopup.OnHintUsed -= HintUsed;
            }

            foreach (var wordInstance in _gameMenuScreen.WordInstances)
            {
                wordInstance.OnShowHint -= ShowHint;
            }
        }

        private void HintClicked()
        {
            _uiManager.PopupsManager.ShowPopup(PopupType.Hint);
            _hintPopup = _uiManager.PopupsManager.GetPopup(PopupType.Hint) as HintPopup;
            GameWord wordToHint = GetHint();
            if (_hintPopup != null)
            {
                _hintPopup.Initialize(wordToHint.Word, wordToHint.Description, false,
                    _gameDataManager.GamePlayConfig.HintCost);
                _hintPopup.OnHintUsed += HintUsed;
            }
        }

        private void HintUsed(string word)
        {
            foreach (var wordInstance in _gameMenuScreen.WordInstances)
            {
                if (wordInstance.Word == word)
                {
                    _wordsWithHint.Add(word);
                    wordInstance.UnlockHint();
                    wordInstance.OnShowHint += ShowHint;
                }
            }
        }

        private void ShowHint(string word, bool isUnlocked)
        {
            _uiManager.PopupsManager.ShowPopup(PopupType.Hint);
            _hintPopup = _uiManager.PopupsManager.GetPopup(PopupType.Hint) as HintPopup;
            GameWord wordToHint = GetHint(word);
            if (_hintPopup != null)
            {
                _hintPopup.Initialize(wordToHint.Word, wordToHint.Description, isUnlocked,
                    _gameDataManager.GamePlayConfig.HintCost);
                _hintPopup.OnHintUsed += HintUsed;
            }
        }

        private GameWord GetHint(string word)
        {
            foreach (GameWord gameWord in _levelWords)
            {
                if (gameWord.Word == word)
                {
                    return gameWord;
                }
            }

            return null;
        }

        private GameWord GetHint()
        {
            var lockedWords = new List<GameWord>();

            foreach (GameWord word in _levelWords)
            {
                if (!_unlockedWords.Contains(word.Word))
                {
                    lockedWords.Add(word);
                }
            }

            if (lockedWords.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, lockedWords.Count);
                return lockedWords[randomIndex];
            }

            return null;
        }

        private void CheckWrittenWord()
        {
            _gameMenuScreen.ActivatePressedButtons();
            for (var index = 0; index < _levelWords.Count; index++)
            {
                GameWord word = _levelWords[index];
                WordControl wordInstance = _gameMenuScreen.WordInstances[index];

                if (word.Word == _written && !wordInstance.IsUnlocked)
                {
                    _unlockedWords.Add(word.Word);
                    wordInstance.UnlockWord();
                    _gameMenuScreen.UnlockWord(wordInstance);
                    _gameMenuScreen.UpdateShowedWordCount(_unlockedWords.Count);
                    ClearWord();

                    _currencyService.GetCurrencyByType(CurrencyType.Coin)
                        .EarnCurrency(_gameDataManager.GamePlayConfig.CoinCountForUnlockedWord);
                    return;
                }
            }

            ClearWord();
        }

        private void AddSign(char sign)
        {
            _clockService.StartTimer(ClockConstants.WORD_WRITTEN,
                _gameDataManager.GamePlayConfig.SecondsToCheckWord, CheckWrittenWord);
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
                    _gameDataManager.GamePlayConfig.SecondsToCheckWord, CheckWrittenWord);
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