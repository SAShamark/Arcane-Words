using System.Collections.Generic;
using Game.Data;
using UI.Core;
using UI.Popups.Core;
using UI.Popups.Hint;
using UI.Screens.GameMenu;

namespace Game.GamePlay
{
    public class HintManager
    {
        private readonly IUIManager _uiManager;
        private readonly GameDataManager _gameDataManager;
        private readonly GameMenuScreen _gameMenuScreen;
        private readonly List<GameWord> _levelWords;
        private readonly List<string> _unlockedWords;
        private readonly List<WordControl> _wordInstances;
        private HintPopup _hintPopup;
        public List<string> WordsWithHint { get; internal set; }
        private readonly bool _isLevelCompleted;

        public HintManager(IUIManager uiManager, GameDataManager gameDataManager, List<WordControl> wordInstances,
            List<GameWord> levelWords, List<string> unlockedWords, GameMenuScreen gameMenuScreen, bool isLevelCompleted)
        {
            _uiManager = uiManager;
            _gameDataManager = gameDataManager;
            _gameMenuScreen = gameMenuScreen;
            _levelWords = levelWords;
            _unlockedWords = unlockedWords;
            _wordInstances = wordInstances;
            _isLevelCompleted = isLevelCompleted;
        }

        internal void UpdateHintButtonActivity()
        {
            bool areAllHintsUsed = _levelWords.Count - _unlockedWords.Count == WordsWithHint.Count;
            _gameMenuScreen.ChangeHintButtonActivity(!areAllHintsUsed);
        }

        private void RequestHint()
        {
            if (!_isLevelCompleted)
            {
                _uiManager.PopupsManager.ShowPopup(PopupType.Hint);
                _hintPopup = _uiManager.PopupsManager.GetPopup(PopupType.Hint) as HintPopup;
                GameWord wordToHint = GetHint();
                if (wordToHint != null && _hintPopup != null)
                {
                    _hintPopup.Initialize(wordToHint.Word, wordToHint.Description, false, false,
                        _gameDataManager.GamePlayConfig.HintCost);
                    _hintPopup.OnHintUsed += HintUsed;
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
                _hintPopup.Initialize(wordToHint.Word, wordToHint.Description, true, isUnlocked,
                    _gameDataManager.GamePlayConfig.HintCost);
                _hintPopup.OnHintUsed += HintUsed;
            }
        }

        private void HintUsed(string word)
        {
            foreach (var wordInstance in _wordInstances)
            {
                if (wordInstance.Word == word)
                {
                    WordsWithHint.Add(word);
                    wordInstance.UnlockHint();
                }
            }

            UpdateHintButtonActivity();
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
                if (!_unlockedWords.Contains(word.Word) && !WordsWithHint.Contains(word.Word))
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

        public void Subscribe()
        {
            _gameMenuScreen.OnHintRequested += RequestHint;
            _gameMenuScreen.OnHintProcessed += ShowHint;
        }

        public void Unsubscribe()
        {
            _gameMenuScreen.OnHintRequested -= RequestHint;
            _gameMenuScreen.OnHintProcessed -= ShowHint;
            if (_hintPopup != null)
            {
                _hintPopup.OnHintUsed -= HintUsed;
            }

            foreach (var wordInstance in _gameMenuScreen.WordInstances)
            {
                wordInstance.OnShowHint -= ShowHint;
            }
        }
    }
}