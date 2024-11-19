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
        private GameObject _levelInProgress;

        [SerializeField]
        private GameObject _levelCompleted;

        [SerializeField]
        private TMP_Text _levelNameText;

        [SerializeField]
        private TMP_Text _progressText;

        [SerializeField]
        private Image _progressImage;

        private int _index;

        public event Action<int> OnButtonClicked;

        public void Draw(int index, bool isUnlocked, bool isCompleted, Sprite progressSprite = null, string name = "",
            int progress = 0)
        {
            _index = index;
            _button.onClick.AddListener(ButtonClicked);
            _lockedLevel.SetActive(!isUnlocked);
            _unlockedLevel.SetActive(isUnlocked);
            _levelCompleted.SetActive(isCompleted);
            _levelInProgress.SetActive(!isCompleted && isUnlocked);

            _levelNameText.text = name;
            _progressText.text = $"{progress}%";

            if (isUnlocked && !isCompleted)
            {
                _progressImage.sprite = progressSprite;
            }
        }

        private void ButtonClicked()
        {
            OnButtonClicked?.Invoke(_index);
        }
    }
}