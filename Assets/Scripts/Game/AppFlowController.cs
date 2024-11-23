using Game.Data;
using Services.Clock;
using Services.Currencies;
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
        private LevelsConfig _levelsConfig;
        private GamePlayConfig _gamePlayConfig;

        private IUIManager _uiManager;
        private IClockService _clockService;
        private IStorageService _storageService;
        private CurrencyService _currencyService;
        private GameDataManager _gameDataManager;
        private GameLauncher _gameLauncher;
        private MainMenuScreen _mainMenu;

        [Inject]
        private void Construct(IUIManager uiManager, IClockService clockService, IStorageService storageService,
            CurrencyService currencyService, LevelsConfig levelsConfig, GamePlayConfig gamePlayConfig)
        {
            _uiManager = uiManager;
            _clockService = clockService;
            _storageService = storageService;
            _currencyService = currencyService;
            _levelsConfig = levelsConfig;
            _gamePlayConfig = gamePlayConfig;
        }

        private void Start()
        {
            InitializeGameDataService();
            InitializeMainMenu();

            _gameLauncher = new GameLauncher(_uiManager, _clockService, _gameDataManager,_currencyService);
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
                _mainMenu.Init(_levelsConfig, _gameDataManager);
                _mainMenu.OnLevelButtonClicked += LevelButtonClicked;
            }
        }

        private void InitializeGameDataService()
        {
            var gameDataService = new GameDataManager(_gamePlayConfig, _levelsConfig, _storageService);
            gameDataService.Initialize();
            _gameDataManager = gameDataService;
        }

        private void LevelButtonClicked(int index)
        {
            _gameLauncher.LaunchGame(index);
        }
    }
}