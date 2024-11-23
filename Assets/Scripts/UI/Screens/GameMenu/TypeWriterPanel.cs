using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        private readonly List<Button> _pressedButtons = new();
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
                button.OnButtonClicked += ButtonClicked;
            }

            _eraseKeyButton.OnButtonClicked += EraseSign;
            _clearKeyButton.OnButtonClicked += ClearWord;
        }

        private void Unsubscribe()
        {
            foreach (KeyButton button in _keyButtons)
            {
                button.OnButtonClicked -= ButtonClicked;
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