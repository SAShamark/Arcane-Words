using System;
using System.Collections.Generic;
using Game;
using UI.Core;
using UI.Screens.Core;
using UI.Screens.GameMenu;
using UnityEngine;

namespace UI.Screens.MainMenu
{
    public class MainMenuScreen : BaseScreen
    {
        [SerializeField]
        private Transform _levelsContent;

        [SerializeField]
        private LevelButton _levelButton;

        private LevelsConfig _levelsConfig;
        private GameDataService _gameDataService;

        private readonly List<LevelButton> _levelButtons = new();
        public event Action<int> OnLevelButtonClicked;

        private void OnDestroy()
        {
            foreach (LevelButton button in _levelButtons)
            {
                button.OnButtonClicked -= LevelButtonClicked;
            }
        }

        public void Init(LevelsConfig levelsConfig, GameDataService gameDataService)
        {
            _levelsConfig = levelsConfig;
            _gameDataService = gameDataService;
            Draw();
        }

        private void Draw()
        {
            for (var index = 0; index < _levelsConfig.LevelsOnPage; index++)
            {
                LevelButton button = Instantiate(_levelButton, _levelsContent);
                _levelButtons.Add(button);
                button.OnButtonClicked += LevelButtonClicked;
                if (_gameDataService.Levels.Count > index)
                {
                    button.Draw(index, true, false, _levelsConfig.LevelMedals[0].Medal, _gameDataService.Levels[index],
                        0);
                }
                else
                {
                    button.Draw(index, false, false);
                }
            }
        }

        private void LevelButtonClicked(int index)
        {
            OnLevelButtonClicked?.Invoke(index);
        }
    }
}