using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI.Screens.GameMenu
{
    public class TypeWriterPanel : MonoBehaviour
    {
        [SerializeField]
        private List<KeyButton> _keyButtons;

        [SerializeField]
        private KeyButton _clearKeyButton;

        [SerializeField]
        private KeyButton _eraseKeyButton;

        [SerializeField]
        private TMP_Text _writtenText;

        private readonly List<KeyButton> _pressedButtons = new();
        public event Action<char> OnAddSign;
        public event Action OnEraseSign;
        public event Action OnClearWord;

        private void Awake()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            foreach (KeyButton button in _keyButtons)
            {
                button.OnButtonClicked += SignButtonClicked;
            }

            _eraseKeyButton.OnButtonClicked += EraseSign;
            _clearKeyButton.OnButtonClicked += ClearWord;
        }

        private void Unsubscribe()
        {
            foreach (KeyButton button in _keyButtons)
            {
                button.OnButtonClicked -= SignButtonClicked;
            }

            _eraseKeyButton.OnButtonClicked -= EraseSign;
            _clearKeyButton.OnButtonClicked -= ClearWord;
        }

        public void Init(string header)
        {
            _writtenText.text = string.Empty;

            InitButtons(header);
        }

        private void InitButtons(string header)
        {
            for (var i = 0; i < _keyButtons.Count; i++)
            {
                char character = i < header.Length ? header[i] : ' ';
                _keyButtons[i].Init(character);
            }

            _eraseKeyButton.Init('\0');
            _clearKeyButton.Init('\0');
        }

        public void UpdateWrittenText(string text)
        {
            _writtenText.text = text;
        }

        public void ActivatePressedButtons()
        {
            foreach (KeyButton button in _pressedButtons)
            {
                button.Button.interactable = true;
            }

            _pressedButtons.Clear();
        }

        private void ActivatePressedButton()
        {
            KeyButton keyButton = _pressedButtons.Last();
            keyButton.Button.interactable = true;
            _pressedButtons.Remove(keyButton);
        }

        private void SignButtonClicked(KeyButton button)
        {
            button.Button.interactable = false;
            _pressedButtons.Add(button);
            AddSign(button.Sign);
        }

        private void AddSign(char sign)
        {
            OnAddSign?.Invoke(sign);
        }

        private void EraseSign(KeyButton button)
        {
            ActivatePressedButton();
            OnEraseSign?.Invoke();
        }

        private void ClearWord(KeyButton button)
        {
            ActivatePressedButtons();
            OnClearWord?.Invoke();
        }
    }
}