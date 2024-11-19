using UI.Core;
using UI.Screens.Core;
using UI.Screens.MainMenu;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class AppFlowController : MonoBehaviour
    {
        [SerializeField]
        private LevelsConfig _levelsConfig;

        private ProjectCanvas _projectCanvas;

        private MainMenuScreen _mainMenu;

        [Inject]
        private void Construct(ProjectCanvas projectCanvas)
        {
            _projectCanvas = projectCanvas;
        }

        private void Start()
        {
            _projectCanvas.ScreensManager.ShowScreen(ScreenType.MainMenu);
            _mainMenu = _projectCanvas.ScreensManager.GetScreen(ScreenType.MainMenu) as MainMenuScreen;
            var gameDataService = new GameDataService();
            gameDataService.Initialize(_levelsConfig);
            _mainMenu?.Init(_levelsConfig, gameDataService.Levels);
        }
    }
}