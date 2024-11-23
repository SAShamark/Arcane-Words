using System;
using Services.Clock;
using Services.Storage;
using UI.Core;
using UI.Screens.Core;
using UI.Screens.GameMenu;

namespace Game
{
    public sealed class GameLauncher: IDisposable
    {
        private GameMenuScreen _gameMenu;
        private GameController _gameController;
        
        private readonly IUIManager _uiManager;
        private readonly IClockService _clockService;
        private readonly IStorageService _storageService;
        private readonly GameDataService _gameDataService;
        

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
            _gameController = new GameController(_clockService);
        }

        public void LaunchGame(int levelIndex)
        {
            _uiManager.ScreensManager.ShowScreen(ScreenType.GameMenu);
            _gameMenu = _uiManager.ScreensManager.GetScreen(ScreenType.GameMenu) as GameMenuScreen;
            string level = _gameDataService.Levels[levelIndex];
            _gameController.StartGame(_gameMenu, _gameDataService, level);
        }

        public void Dispose()
        {
            _gameController?.Dispose();
        }
    }
}