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
            Draw();
        }

        private void Draw()
        {
            _text.text = new string('-', Word.Length);
        }

        public void UnlockWord()
        {
            IsUnlocked = true;
            _text.text = Word;
        }

        public void Reset()
        {
            _text.text = "";
            IsUnlocked = false;
        }
    }
}