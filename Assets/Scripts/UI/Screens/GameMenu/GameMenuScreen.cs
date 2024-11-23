using System;
using System.Collections.Generic;
using Game;
using Services.Clock;
using Services.ObjectPooling;
using TMPro;
using UI.Screens.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screens.GameMenu
{
    public class GameMenuScreen : BaseScreen
    {
        [SerializeField]
        private List<KeyButton> _keyButtons;

        [SerializeField]
        private KeyButton _clearKeyButton;

        [SerializeField]
        private KeyButton _eraseKeyButton;

        [SerializeField]
        private TMP_Text _writtenText;

        [SerializeField]
        private TMP_Text _headerText;

        [SerializeField]
        private Transform _typePaperContent;

        [SerializeField]
        private WordControl _wordPrefab;

        [SerializeField]
        private TMP_Text _wordsCountText;

        [SerializeField]
        private TMP_Text _hintCountText;

        [SerializeField]
        private TMP_Text _stopwatchText;

        private ObjectPool<WordControl> _objectPool;
        private IClockService _clockService;
        private List<GameWord> _levelWords;
        private List<string> _unlockedWords;
        private readonly List<Button> _pressedButtons = new();
        private float _elapsedTime;
        public List<WordControl> WordInstances { get; private set; } = new();
        public event Action<char> OnAddSign;
        public event Action OnEraseSign;
        public event Action OnClearWord;
        public event Action OnExit;

        [Inject]
        private void Construct(IClockService clockService)
        {
            _clockService = clockService;
        }

        private void Update()
        {
            UpdateStopwatch();
        }

        public void Init(string header, List<GameWord> levelWords, List<string> unlockedWords, float elapsedTime)
        {
            _unlockedWords = unlockedWords;
            _elapsedTime = elapsedTime;
            _headerText.text = header;
            _levelWords = levelWords;
            _writtenText.text = string.Empty;
            _objectPool = new ObjectPool<WordControl>(_wordPrefab, 5, _typePaperContent);


            InitButtons(header);
            DrawWords(levelWords);
            Subscribe();
        }

        public void Unsubscribe()
        {
            foreach (KeyButton key in _keyButtons)
            {
                key.OnButtonClicked -= ButtonClicked;
            }

            _eraseKeyButton.OnButtonClicked -= EraseSign;
            _clearKeyButton.OnButtonClicked -= ClearWord;
        }

        private void Subscribe()
        {
            foreach (KeyButton button in _keyButtons)
            {
                button.OnButtonClicked += ButtonClicked;
            }

            _eraseKeyButton.OnButtonClicked += EraseSign;
            _clearKeyButton.OnButtonClicked += ClearWord;
        }

        private void InitButtons(string header)
        {
            _backButton.onClick.AddListener(() => OnExit?.Invoke());

            if (_keyButtons.Count != header.Length)
            {
                Debug.LogWarning("Mismatch between key buttons count and header length.");
            }

            for (var i = 0; i < _keyButtons.Count; i++)
            {
                char character = i < header.Length ? header[i] : ' ';
                _keyButtons[i].Init(character);
            }

            _eraseKeyButton.Init('\0');
            _clearKeyButton.Init('\0');
        }

        private void DrawWords(List<GameWord> levelWords)
        {
            foreach (GameWord levelWord in levelWords)
            {
                WordControl wordInstance = _objectPool.GetFreeElement();
                wordInstance.Init(levelWord.Word);
                foreach (string unlockedWord in _unlockedWords)
                {
                    if (levelWord.Word == unlockedWord)
                    {
                        wordInstance.UnlockWord();
                    }
                }

                WordInstances.Add(wordInstance);
            }
        }

        public override void Hide()
        {
            base.Hide();
            foreach (WordControl word in WordInstances)
            {
                _objectPool.TurnOffObject(word);
            }

            WordInstances.Clear();
        }

        private void UpdateStopwatch()
        {
            float elapsedTime = _elapsedTime + _clockService.CalculateElapsedTime(ClockConstants.GAME_TIMER);
            _stopwatchText.text = _clockService.FormatToTime(elapsedTime);
        }

        public void UpdateShowedWordCount(int showedCount)
        {
            _wordsCountText.text = $"{showedCount}/{_levelWords.Count}";
        }

        public void ActivatePressedButtons()
        {
            foreach (var button in _pressedButtons)
            {
                button.interactable = true;
            }

            _pressedButtons.Clear();
        }

        private void ButtonClicked(KeyButton button)
        {
            _pressedButtons.Add(button.Button);
            AddSign(button.Sign);
        }

        public void UpdateWrittenText(string text)
        {
            _writtenText.text = text;
        }

        private void AddSign(char sign)
        {
            OnAddSign?.Invoke(sign);
        }

        private void EraseSign(KeyButton button)
        {
            _pressedButtons.Add(button.Button);
            OnEraseSign?.Invoke();
        }

        private void ClearWord(KeyButton button)
        {
            _pressedButtons.Add(button.Button);
            OnClearWord?.Invoke();
        }
    }
}