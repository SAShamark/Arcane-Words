using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Audio.Data;
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
        private IAudioManager _audioManager;
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

        public void Init(string header, IAudioManager audioManager)
        {
            _audioManager = audioManager;
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
                button.ResetIsPressed();
                _audioManager.Play(AudioGroupType.UiSounds, "UnpressKey");
            }

            _pressedButtons.Clear();
        }

        private void ActivateLastPressedButton()
        {
            if (_pressedButtons.Count > 0)
            {
                KeyButton button = _pressedButtons.Last();
                button.Button.interactable = true;
                button.ResetIsPressed();
                _audioManager.Play(AudioGroupType.UiSounds, "UnpressKey");
                _pressedButtons.Remove(button);
            }
        }

        private void Subscribe()
        {
            foreach (KeyButton button in _keyButtons)
            {
                button.OnButtonClicked += SignButtonClicked;
            }

            _eraseKeyButton.OnButtonClicked += EraseSignButtonClicked;
            _clearKeyButton.OnButtonClicked += ClearWordButtonClicked;
        }

        private void Unsubscribe()
        {
            foreach (KeyButton button in _keyButtons)
            {
                button.OnButtonClicked -= SignButtonClicked;
            }

            _eraseKeyButton.OnButtonClicked -= EraseSignButtonClicked;
            _clearKeyButton.OnButtonClicked -= ClearWordButtonClicked;
        }

        private void SignButtonClicked(KeyButton button)
        {
            _audioManager.Play(AudioGroupType.UiSounds, "PressKey");
            button.Button.interactable = false;
            _pressedButtons.Add(button);
            OnAddSign?.Invoke(button.Sign);
        }

        private void EraseSignButtonClicked(KeyButton button)
        {
            _audioManager.Play(AudioGroupType.UiSounds, "PressKey");
            ActivateLastPressedButton();
            OnEraseSign?.Invoke();
        }

        private void ClearWordButtonClicked(KeyButton button)
        {
            _audioManager.Play(AudioGroupType.UiSounds, "PressKey");
            ActivatePressedButtons();
            OnClearWord?.Invoke();
        }
    }
}