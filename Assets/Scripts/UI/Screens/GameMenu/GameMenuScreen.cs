using System;
using System.Collections.Generic;
using Game;
using Services.Clock;
using Services.ObjectPooling;
using TMPro;
using UI.Screens.Core;
using UnityEngine;
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

        private void OnDestroy()
        {
            foreach (KeyButton key in _keyButtons)
            {
                key.OnButtonClicked -= ButtonClicked;
            }

            _eraseKeyButton.OnButtonClicked -= EraseSign;
            _eraseKeyButton.OnButtonClicked -= ButtonClicked;
            _clearKeyButton.OnButtonClicked -= ClearWord;
            _clearKeyButton.OnButtonClicked -= ButtonClicked;
        }

        public void Init(string header, List<GameWord> levelWords)
        {
            _backButton.onClick.AddListener(()=>OnExit?.Invoke());
            _headerText.text = header;
            _writtenText.text = string.Empty;
            _levelWords = levelWords;
            _objectPool = new ObjectPool<WordControl>(_wordPrefab, 5, _typePaperContent);
            InitButtons(header);
            DrawWords(levelWords);
        }

        private void UpdateStopwatch()
        {
            float elapsedTime = _clockService.CalculateElapsedTime(ClockConstants.GAME_TIMER);
            _stopwatchText.text = _clockService.FormatToTime(elapsedTime);
        }

        public void UpdateShowedWordCount(int showedCount)
        {
            _wordsCountText.text = $"{showedCount}/{_levelWords.Count}";
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

        private void DrawWords(List<GameWord> levelWords)
        {
            foreach (GameWord levelWord in levelWords)
            {
                WordControl wordInstance = _objectPool.GetFreeElement();
                wordInstance.Init(levelWord.Word);
                WordInstances.Add(wordInstance);
            }
        }

        private void InitButtons(string header)
        {
            if (_keyButtons.Count != header.Length)
            {
                Debug.LogWarning("Mismatch between key buttons count and header length.");
            }

            for (var i = 0; i < _keyButtons.Count; i++)
            {
                char character = i < header.Length ? header[i] : ' ';
                _keyButtons[i].Init(character);
                _keyButtons[i].OnButtonClicked += ButtonClicked;
            }

            _eraseKeyButton.Init('\0');
            _clearKeyButton.Init('\0');

            _eraseKeyButton.OnButtonClicked += EraseSign;
            _eraseKeyButton.OnButtonClicked += ButtonClicked;
            _clearKeyButton.OnButtonClicked += ClearWord;
            _clearKeyButton.OnButtonClicked += ButtonClicked;
        }

        private void ButtonClicked(char sign)
        {
            AddSign(sign);
        }

        public void UpdateWrittenText(string text)
        {
            _writtenText.text = text;
        }

        private void AddSign(char sign)
        {
            OnAddSign?.Invoke(sign);
        }

        private void EraseSign(char _)
        {
            OnEraseSign?.Invoke();
        }

        private void ClearWord(char _)
        {
            OnClearWord?.Invoke();
        }
    }
}