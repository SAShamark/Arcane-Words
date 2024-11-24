using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.MainMenu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _lockedLevel;

        [SerializeField]
        private GameObject _unlockedLevel;

        [SerializeField]
        private TMP_Text _levelNameText;

        [SerializeField]
        private TMP_Text _progressText;

        [SerializeField]
        private Image _progressImage;

        private int _index;

        public event Action<int> OnButtonClicked;

        public void Draw(int index, bool isUnlocked, bool isCompleted, string name = "",
            int progress = 0, Sprite progressSprite = null)
        {
            _index = index;
            _button.onClick.AddListener(ButtonClicked);
            _levelNameText.text = name;
            UpdateVisual(isUnlocked, isCompleted, progress, progressSprite);
        }

        public void UpdateVisual(bool isUnlocked, bool isCompleted, int progress, Sprite progressSprite)
        {
            _button.interactable = isUnlocked;
            _lockedLevel.SetActive(!isUnlocked);
            _unlockedLevel.SetActive(isUnlocked);
            _progressText.text = $"{progress}%";

            if (isCompleted)
            {
                _progressImage.gameObject.SetActive(true);
                _progressText.gameObject.SetActive(false);
                _progressImage.sprite = progressSprite;
                return;
            }

            if (isUnlocked)
            {
                if (progressSprite == null)
                {
                    _progressImage.gameObject.SetActive(false);
                    _progressText.gameObject.SetActive(true);
                }
                else
                {
                    _progressImage.gameObject.SetActive(true);
                    _progressImage.sprite = progressSprite;
                }
            }
        }

        private void ButtonClicked()
        {
            OnButtonClicked?.Invoke(_index);
        }
    }
}