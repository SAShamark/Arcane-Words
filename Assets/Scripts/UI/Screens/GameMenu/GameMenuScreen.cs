using System;
using System.Collections.Generic;
using Game;
using Game.Data;
using Services.Clock;
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
        private TMP_Text _headerText;

        [SerializeField]
        private TMP_Text _wordsCountText;

        [SerializeField]
        private TMP_Text _hintCountText;

        [SerializeField]
        private Button _hintButton;

        [SerializeField]
        private TMP_Text _stopwatchText;

        [SerializeField]
        private TypePaperPanel _typePaperPanel;

        [SerializeField]
        private TypeWriterPanel _typeWriterPanel;

        private IClockService _clockService;
        private float _elapsedTime;
        private List<GameWord> _levelWords;
        private List<string> _wordsWithHint;
        public List<WordControl> WordInstances => _typePaperPanel.WordInstances;

        public event Action<char> OnAddSign;
        public event Action OnEraseSign;
        public event Action OnClearWord;
        public event Action OnExit;
        public event Action OnHint;

        [Inject]
        private void Construct(IClockService clockService)
        {
            _clockService = clockService;
        }

        private void Awake()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Update()
        {
            UpdateStopwatch();
        }

        public void Init(string header, List<GameWord> levelWords, List<string> unlockedWords,
            List<string> wordsWithHint, float elapsedTime, int hintCount)
        {
            _elapsedTime = elapsedTime;
            _levelWords = levelWords;
            _wordsWithHint = wordsWithHint;
            _headerText.text = header;
            _hintCountText.text = hintCount.ToString();
            _typePaperPanel.Init(_levelWords,_wordsWithHint, unlockedWords);
            _typeWriterPanel.Init(header);
        }

        private void Subscribe()
        {
            _backButton.onClick.AddListener(() => OnExit?.Invoke());
            _hintButton.onClick.AddListener(() => OnHint?.Invoke());

            _typeWriterPanel.OnAddSign += AddSign;
            _typeWriterPanel.OnEraseSign += EraseSign;
            _typeWriterPanel.OnClearWord += ClearWord;
        }

        private void Unsubscribe()
        {
            _typeWriterPanel.OnAddSign -= AddSign;
            _typeWriterPanel.OnEraseSign -= EraseSign;
            _typeWriterPanel.OnClearWord -= ClearWord;
            _typePaperPanel.Unsubscribe();
        }

        public void UpdateWrittenText(string written) => _typeWriterPanel.UpdateWrittenText(written);
        private void AddSign(char sign) => OnAddSign?.Invoke(sign);

        private void EraseSign() => OnEraseSign?.Invoke();

        private void ClearWord() => OnClearWord?.Invoke();

        public void UnlockWord(WordControl unlockedWord)
        {
            _typePaperPanel.ScrollToWord(unlockedWord);
        }

        public void UpdateShowedWordCount(int showedCount) =>
            _wordsCountText.text = $"{showedCount}/{_levelWords.Count}";


        private void UpdateStopwatch()
        {
            float elapsedTime = _elapsedTime + _clockService.CalculateElapsedTime(ClockConstants.GAME_TIMER);
            _stopwatchText.text = _clockService.FormatToTime(elapsedTime);
        }

        public override void Hide()
        {
            base.Hide();
            _typePaperPanel.ClearWords();
        }

        public void ActivatePressedButtons() => _typeWriterPanel.ActivatePressedButtons();
    }
}