﻿using System;
using System.Collections.Generic;
using Game.Data;
using UI.Core.Data;
using UI.Screens.Core;
using UI.Screens.MainMenu.Data;
using UnityEngine;

namespace UI.Screens.MainMenu
{
    public class MainMenuScreen : BaseScreen
    {
        [SerializeField]
        private Transform _levelsContent;

        [SerializeField]
        private LevelButton _levelButtonPrefab;

        private LevelsConfig _levelsConfig;
        private GameDataManager _gameDataManager;

        private readonly List<LevelButton> _levelButtons = new();
        public event Action<int> OnLevelButtonClicked;

        private void OnDestroy()
        {
            foreach (LevelButton button in _levelButtons)
            {
                button.OnButtonClicked -= LevelButtonClicked;
            }
        }

        public void Init(LevelsConfig levelsConfig, GameDataManager gameDataManager)
        {
            _levelsConfig = levelsConfig;
            _gameDataManager = gameDataManager;
            DrawLevelButtons();
        }

        public override void Show()
        {
            base.Show();
            UpdateLevelButtons();
        }

        private void DrawLevelButtons()
        {
            for (var index = 0; index < _levelsConfig.LevelsOnPage; index++)
            {
                CreateLevelButton(index);
            }
        }

        private void UpdateLevelButtons()
        {
            for (var index = 0; index < _levelButtons.Count; index++)
            {
                LevelButton button = _levelButtons[index];
                if (index < _gameDataManager.Levels.Count)
                {
                    (string _, bool isUnlocked, bool isCompleted, int progress, Sprite medal) = GetLevelData(index);
                    button.UpdateVisual(isUnlocked, isCompleted, progress, medal);
                }
                else
                {
                    button.UpdateVisual(false, false, 0, null);
                }
            }
        }

        private void CreateLevelButton(int index)
        {
            LevelButton button = Instantiate(_levelButtonPrefab, _levelsContent);
            button.OnButtonClicked += LevelButtonClicked;

            if (index < _gameDataManager.Levels.Count)
            {
                (string level, bool isUnlocked, bool isCompleted, int progress, Sprite medal) = GetLevelData(index);
                button.Draw(index, isUnlocked, isCompleted, level, progress, medal);
            }
            else
            {
                button.Draw(index, false, false);
            }

            _levelButtons.Add(button);
        }

        private (string level, bool isUnlocked, bool isCompleted, int progress, Sprite medal) GetLevelData(int index)
        {
            string level = _gameDataManager.Levels[index];
            int progress = _gameDataManager.CalculateLevelProgress(level);
            Sprite medal = GetMedalSprite(progress);
            var isUnlocked = true;
            bool isCompleted = progress == 100;

            return (level, isUnlocked, isCompleted, progress, medal);
        }


        private Sprite GetMedalSprite(int levelProgress)
        {
            Sprite progressMedal = null;
            foreach (LevelMedals medal in _levelsConfig.LevelMedals)
            {
                if (levelProgress >= medal.PercentToMedal.x && levelProgress <= medal.PercentToMedal.y)
                {
                    progressMedal = medal.Sprite;
                }
            }

            return progressMedal;
        }

        private void LevelButtonClicked(int index)
        {
            OnLevelButtonClicked?.Invoke(index);
        }
    }
}