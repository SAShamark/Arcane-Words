using System;
using Services.ObjectPooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.GameMenu
{
    public class WordControl : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private TMP_Text _text;

        [SerializeField]
        private Button _button;

        public string Word { get; private set; }
        public bool IsUnlocked { get; private set; }
        public GameObject GameObject { get; private set; }
        public event Action<IPoolable> OnReturnToPool;
        public event Action<string, bool> OnShowHint;

        public void Init(string word)
        {
            GameObject = gameObject;
            Word = word;

            _button.onClick.AddListener(() => OnShowHint?.Invoke(Word, IsUnlocked));
            DrawLockedWord();
        }

        private void DrawLockedWord()
        {
            _text.text = new string('-', Word.Length);
            IsUnlocked = false;
            _button.interactable = false;
        }

        public void UnlockHint()
        {
            _text.text = new string('+', Word.Length);
            _button.interactable = true;
        }

        public void UnlockWord()
        {
            _text.text = Word;
            IsUnlocked = true;
            _button.interactable = true;
        }

        public void Reset()
        {
            _text.text = "";
            IsUnlocked = false;
        }
    }
}