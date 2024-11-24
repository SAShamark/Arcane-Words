using System.Collections.Generic;
using Game.Data;
using Services.Clock;
using Services.Currencies;
using UI.Screens.GameMenu;

namespace Game.GamePlay
{
    public class GameWordBuilder
    {
        private string _written;
        private readonly IClockService _clockService;
        private readonly GameDataManager _gameDataManager;
        private readonly GameMenuScreen _gameMenuScreen;
        private readonly List<GameWord> _levelWords;
        private readonly List<string> _unlockedWords;
        private readonly CurrencyService _currencyService;

        public GameWordBuilder(CurrencyService currencyService, IClockService clockService,
            GameDataManager gameDataManager, GameMenuScreen gameMenuScreen, List<string> unlockedWords,
            List<GameWord> levelWords)
        {
            _clockService = clockService;
            _gameDataManager = gameDataManager;
            _gameMenuScreen = gameMenuScreen;
            _levelWords = levelWords;
            _unlockedWords = unlockedWords;
            _currencyService = currencyService;
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
                    UnlockWord(word, wordInstance);
                    return;
                }

                if (word.Word == _written && wordInstance.IsUnlocked)
                {
                    bool wordInstanceIsUnlocked = wordInstance.IsUnlocked;
                    _gameMenuScreen.ScrollToWord(wordInstance, wordInstanceIsUnlocked);
                }
            }

            ClearWord();
        }

        private void UnlockWord(GameWord word, WordControl wordInstance)
        {
            _unlockedWords.Add(word.Word);
            bool wordInstanceIsUnlocked = wordInstance.IsUnlocked;
            _gameMenuScreen.ScrollToWord(wordInstance, wordInstanceIsUnlocked);
            wordInstance.UnlockWord();
            _gameMenuScreen.UpdateShowedWordCount(_unlockedWords.Count);
            ClearWord();

            _currencyService.GetCurrencyByType(CurrencyType.Coin)
                .EarnCurrency(_gameDataManager.GamePlayConfig.CoinCountForUnlockedWord);
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

        internal void ClearWord()
        {
            _written = string.Empty;
            _gameMenuScreen.UpdateWrittenText(_written);
            _clockService.StopTimer(ClockConstants.WORD_WRITTEN);
        }

        public void Subscribe()
        {
            _gameMenuScreen.OnAddSign += AddSign;
            _gameMenuScreen.OnEraseSign += EraseSign;
            _gameMenuScreen.OnClearWord += ClearWord;
        }

        public void Unsubscribe()
        {
            _gameMenuScreen.OnAddSign -= AddSign;
            _gameMenuScreen.OnEraseSign -= EraseSign;
            _gameMenuScreen.OnClearWord -= ClearWord;
        }
    }
}