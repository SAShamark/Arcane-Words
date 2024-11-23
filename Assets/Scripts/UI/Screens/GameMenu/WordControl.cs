using System;
using Services.ObjectPooling;
using TMPro;
using UnityEngine;

namespace UI.Screens.GameMenu
{
    public class WordControl : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private TMP_Text _text;

        public string Word { get; private set; }
        public bool IsUnlocked { get; private set; }
        public GameObject GameObject { get; private set; }
        public event Action<IPoolable> OnReturnToPool;

        public void Init(string word)
        {
            GameObject = gameObject;
            Word = word;
            DrawLockedWord();
        }

        private void DrawLockedWord()
        {
            _text.text = new string('-', Word.Length);
            IsUnlocked = false;
        }

        public void UnlockWord()
        {
            _text.text = Word;
            IsUnlocked = true;
        }

        public void Reset()
        {
            _text.text = "";
            IsUnlocked = false;
        }
    }
}