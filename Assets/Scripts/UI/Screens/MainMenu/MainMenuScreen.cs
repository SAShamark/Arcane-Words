using System.Collections.Generic;
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
        private List<string> _levels;
        private GameMenuScreen _gameMenu;
        
        private readonly List<LevelButton> _levelButtons = new();

        private void OnDestroy()
        {
            foreach (LevelButton button in _levelButtons)
            {
                button.OnButtonClicked -= LevelButtonClicked;
            }
        }

        public void Init(LevelsConfig levelsConfig, List<string> levels)
        {
            _levelsConfig = levelsConfig;
            _levels = levels;
            Draw();
            
        }

        private void Draw()
        {
            for (var index = 0; index < _levelsConfig.LevelsOnPage; index++)
            {
                LevelButton button = Instantiate(_levelButton, _levelsContent);
                _levelButtons.Add(button);
                button.OnButtonClicked += LevelButtonClicked;
                if (_levels.Count > index)
                {
                    button.Draw(index, true, false, _levelsConfig.LevelMedals[0].Medal, _levels[index], 0);
                }
                else
                {
                    button.Draw(index, false, false);
                }
            }
        }

        private void LevelButtonClicked(int index)
        {
            UIManager.ScreensManager.ShowScreen(ScreenType.GameMenu);
            _gameMenu = UIManager.ScreensManager.GetScreen(ScreenType.GameMenu) as GameMenuScreen;
            _gameMenu?.Init(_levels[index]);
        }
    }
}