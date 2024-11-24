using System;
using Game.Data;
using Game.GamePlay;
using Services.Clock;
using Services.Currencies;
using UI.Core;
using UI.Screens.Core;
using UI.Screens.GameMenu;

namespace Game
{
    public sealed class GameLauncher : IDisposable
    {
        private GameMenuScreen _gameMenu;
        private GameController _gameController;

        private readonly IUIManager _uiManager;
        private readonly IClockService _clockService;
        private readonly CurrencyService _currencyService;
        private readonly GameDataManager _gameDataManager;


        public GameLauncher(IUIManager uiManager, IClockService clockService,
            GameDataManager gameDataManager, CurrencyService currencyService)
        {
            _uiManager = uiManager;
            _gameDataManager = gameDataManager;
            _clockService = clockService;
            _currencyService = currencyService;
        }

        public void LaunchGame(int levelIndex)
        {
            _uiManager.ScreensManager.ShowScreen(ScreenType.GameMenu);
            _gameMenu = _uiManager.ScreensManager.GetScreen(ScreenType.GameMenu) as GameMenuScreen;
            string level = _gameDataManager.Levels[levelIndex];

            _gameController = new GameController(_clockService, _currencyService, _uiManager);
            _gameController.StartGame(_gameMenu, _gameDataManager, level);
        }

        public void Dispose()
        {
            _gameController?.Dispose();
        }
    }
}