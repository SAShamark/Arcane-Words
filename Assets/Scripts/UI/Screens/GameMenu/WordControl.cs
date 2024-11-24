using System;
using DG.Tweening;
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

        [Header("Animation Settings")]
        [SerializeField]
        private float _fontSizeIncrease = 1.1f;

        [SerializeField]
        private float _animationDuration = 0.5f;

        [SerializeField]
        private Color _lockedColor = Color.green;

        [SerializeField]
        private Color _unlockedColor = Color.red;

        private Color _originalColor;

        public string Word { get; private set; }
        public bool IsUnlocked { get; private set; }
        public GameObject GameObject { get; private set; }
        public Color LockedColor => _lockedColor;
        public Color UnlockedColor => _unlockedColor;

        public event Action<IPoolable> OnReturnToPool;
        public event Action<string, bool> OnShowHint;

        private void Start()
        {
            _button.onClick.AddListener(HandleShowHint);
            _originalColor = _text.color;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleShowHint);
        }

        public void Init(string word)
        {
            GameObject = gameObject;
            Word = word;

            DrawLockedWord();
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

        private void DrawLockedWord()
        {
            _text.text = new string('-', Word.Length);
            IsUnlocked = false;
            _button.interactable = false;
        }

        internal void AnimateText(Color targetColor)
        {
            float originalFontSize = _text.fontSize;

            _text.DOColor(targetColor, _animationDuration)
                .OnComplete(() => { _text.DOColor(_originalColor, _animationDuration); });

            _text.DOFontSize(originalFontSize * (1 + _fontSizeIncrease), _animationDuration)
                .OnComplete(() => { _text.DOFontSize(originalFontSize, _animationDuration); });
        }

        private void HandleShowHint() => OnShowHint?.Invoke(Word, IsUnlocked);
    }
}