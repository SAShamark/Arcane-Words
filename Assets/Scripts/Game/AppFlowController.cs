using Services.Clock;
using Services.Storage;
using UI.Core;
using UI.Screens.Core;
using UI.Screens.GameMenu;
using UI.Screens.MainMenu;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class AppFlowController : MonoBehaviour
    {
        [SerializeField]
        private LevelsConfig _levelsConfig;

        private IUIManager _uiManager;

        private MainMenuScreen _mainMenu;
        private GameDataService _gameDataService;
        private IClockService _clockService;
        private GameLauncher _gameLauncher;
        private IStorageService _storageService;

        [Inject]
        private void Construct(IUIManager uiManager, IClockService clockService, IStorageService storageService)
        {
            _uiManager = uiManager;
            _clockService = clockService;
            _storageService = storageService;
        }

        private void Start()
        {
            var gameDataService = new GameDataService();
            gameDataService.Initialize(_levelsConfig);
            _gameDataService = gameDataService;

            _uiManager.ScreensManager.ShowScreen(ScreenType.MainMenu);
            _mainMenu = _uiManager.ScreensManager.GetScreen(ScreenType.MainMenu) as MainMenuScreen;
            if (_mainMenu != null)
            {
                _mainMenu.Init(_levelsConfig, gameDataService);
                _mainMenu.OnLevelButtonClicked += LevelButtonClicked;
            }

            _gameLauncher = new GameLauncher(_uiManager, _clockService, _storageService, _gameDataService);
        }

        private void OnDestroy()
        {
            _mainMenu.OnLevelButtonClicked -= LevelButtonClicked;
        }

        private void LevelButtonClicked(int index)
        {
            _gameLauncher.LaunchGame(index);
        }
    }

    public sealed class GameLauncher
    {
        private GameMenuScreen _gameMenu;
        private readonly IClockService _clockService;
        private readonly IUIManager _uiManager;
        private readonly GameDataService _gameDataService;
        private readonly IStorageService _storageService;
        private GameController _gameController;

        public GameLauncher(IUIManager uiManager, IClockService clockService, IStorageService storageService,
            GameDataService gameDataService)
        {
            _uiManager = uiManager;
            _storageService = storageService;
            _gameDataService = gameDataService;
            _clockService = clockService;

            Init();
        }

        private void Init()
        {
            _gameController = new GameController(_clockService, _storageService);
        }

        public void LaunchGame(int levelIndex)
        {
            _uiManager.ScreensManager.ShowScreen(ScreenType.GameMenu);
            _gameMenu = _uiManager.ScreensManager.GetScreen(ScreenType.GameMenu) as GameMenuScreen;
            string level = _gameDataService.Levels[levelIndex];
            _gameController.StartGame(_gameMenu, _gameDataService, level);
        }
    }
}