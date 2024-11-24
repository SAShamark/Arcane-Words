using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Data;
using Services.ObjectPooling;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.GameMenu
{
    public class TypePaperPanel : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private Transform _typePaperContent;

        [SerializeField]
        private WordControl _wordPrefab;

        private ObjectPool<WordControl> _objectPool;
        private List<GameWord> _levelWords;
        private List<string> _unlockedWords;
        private List<string> _wordsWithHint;
        public List<WordControl> WordInstances { get; private set; } = new();
        public event Action<string, bool> OnShowHint;


        private void Awake()
        {
            _objectPool = new ObjectPool<WordControl>(_wordPrefab, 5, _typePaperContent);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void Init(List<GameWord> levelWords, List<string> wordsWithHint, List<string> unlockedWords)
        {
            _levelWords = levelWords;
            _unlockedWords = unlockedWords;
            _wordsWithHint = wordsWithHint;
            DrawWords();
        }

        public void Unsubscribe()
        {
            foreach (WordControl wordInstance in WordInstances)
            {
                wordInstance.OnShowHint -= ShowHint;
            }
        }

        private void DrawWords()
        {
            foreach (GameWord levelWord in _levelWords)
            {
                WordControl wordInstance = _objectPool.GetFreeElement();
                wordInstance.Init(levelWord.Word);

                if (_unlockedWords.Contains(levelWord.Word))
                {
                    wordInstance.UnlockWord();
                }
                else if (_wordsWithHint.Contains(levelWord.Word))
                {
                    wordInstance.UnlockHint();
                }

                wordInstance.OnShowHint += ShowHint;
                WordInstances.Add(wordInstance);
            }
        }


        private void ShowHint(string word, bool isUnlocked) => OnShowHint?.Invoke(word, isUnlocked);


        public void ClearWords()
        {
            foreach (WordControl word in WordInstances)
            {
                _objectPool.TurnOffObject(word);
            }
            Unsubscribe();
            WordInstances.Clear();
        }

        public void ScrollToWord(WordControl targetWord)
        {
            if (WordInstances.Contains(targetWord))
            {
                int index = WordInstances.IndexOf(targetWord);
                float targetNormalizedPosition = 1f - (float)index / (WordInstances.Count - 1);

                _scrollRect.DOVerticalNormalizedPos(targetNormalizedPosition, 0.5f).SetEase(Ease.InOutQuad);
            }
            else
            {
                Debug.LogWarning("Target word not found in WordInstances.");
            }
        }
    }
}