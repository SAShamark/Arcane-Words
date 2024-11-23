using Services.Clock;
using Services.Storage;
using UI.Core;
using UI.Core.Data;
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

        [SerializeField]
        private GamePlayConfig _gamePlayConfig;

        private IUIManager _uiManager;
        private IClockService _clockService;
        private IStorageService _storageService;
        private GameDataService _gameDataService;
        private GameLauncher _gameLauncher;
        private MainMenuScreen _mainMenu;

        [Inject]
        private void Construct(IUIManager uiManager, IClockService clockService, IStorageService storageService)
        {
            _uiManager = uiManager;
            _clockService = clockService;
            _storageService = storageService;
        }

        private void Start()
        {
            InitializeGameDataService();
            InitializeMainMenu();

            _gameLauncher = new GameLauncher(_uiManager, _clockService, _storageService, _gameDataService);
        }

        private void OnDestroy()
        {
            _mainMenu.OnLevelButtonClicked -= LevelButtonClicked;
            _gameLauncher?.Dispose();
        }

        private void InitializeMainMenu()
        {
            _uiManager.ScreensManager.ShowScreen(ScreenType.MainMenu);
            _mainMenu = _uiManager.ScreensManager.GetScreen(ScreenType.MainMenu) as MainMenuScreen;
            if (_mainMenu != null)
            {
                _mainMenu.Init(_levelsConfig, _gameDataService);
                _mainMenu.OnLevelButtonClicked += LevelButtonClicked;
            }
        }

        private void InitializeGameDataService()
        {
            var gameDataService = new GameDataService(_gamePlayConfig, _levelsConfig, _storageService);
            gameDataService.Initialize();
            _gameDataService = gameDataService;
        }

        private void LevelButtonClicked(int index)
        {
            _gameLauncher.LaunchGame(index);
        }
    }
}