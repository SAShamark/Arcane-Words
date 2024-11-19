using System.Collections.Generic;
using Services.Clock;
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
        private GameObject _wordPrefab;

        [SerializeField]
        private TMP_Text _wordsCountText;

        [SerializeField]
        private TMP_Text _hintCountText;

        [SerializeField]
        private TMP_Text _stopwatchText;

        private string _written;
        private IClockService _clockService;

        [Inject]
        private void Construct(IClockService clockService)
        {
            _clockService = clockService;
        }
        private void OnDestroy()
        {
            foreach (var key in _keyButtons)
            {
                key.OnButtonClicked -= AddSign;
            }

            _eraseKeyButton.OnButtonClicked -= EraseSign;
            _clearKeyButton.OnButtonClicked -= ClearWord;
        }

        public void Init(string header)
        {
            _headerText.text = header;
            _writtenText.text = "";
            if (_keyButtons.Count != header.Length)
            {
                Debug.LogWarning("Mismatch between key buttons count and header length.");
            }

            for (var i = 0; i < _keyButtons.Count; i++)
            {
                char character = i < header.Length ? header[i] : ' ';
                _keyButtons[i].Init(character);
                _keyButtons[i].OnButtonClicked += AddSign;
            }

            _eraseKeyButton.Init('\0');
            _clearKeyButton.Init('\0');

            _eraseKeyButton.OnButtonClicked += EraseSign;
            _clearKeyButton.OnButtonClicked += ClearWord;
        }

        private void AddSign(char sign)
        {
            _written += sign;
            _writtenText.text = _written;
        }

        private void EraseSign(char _)
        {
            if (!string.IsNullOrEmpty(_written))
            {
                _written = _writtenText.text.Substring(0, _writtenText.text.Length - 1);
                _writtenText.text = _written;
            }
        }

        private void ClearWord(char _)
        {
            _written = string.Empty;
            _writtenText.text = _written;
        }
    }
}