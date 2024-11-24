using UI.Popups.Hint;
using UI.Screens.Core;
using UI.Screens.GameMenu;
using UI.Screens.MainMenu;
using UnityEngine;

namespace Game
{
    public class UIIManager : MonoBehaviour
    {
        [SerializeField]
        private MainMenuScreen _mainMenuScreen;

        [SerializeField]
        private GameMenuScreen _gameMenuScreen;

        [SerializeField]
        private HintPopup _hintPopup;

        public MainMenuScreen MainMenuScreen => _mainMenuScreen;

        public GameMenuScreen GameMenuScreen => _gameMenuScreen;

        public HintPopup HintPopup => _hintPopup;

        public void ActivateScreen(ScreenType screenType)
        {
            switch (screenType)
            {
                case ScreenType.MainMenu:
                    _mainMenuScreen.gameObject.SetActive(true);
                    _gameMenuScreen.gameObject.SetActive(false);
                    break;
                case ScreenType.GameMenu:
                    _gameMenuScreen.gameObject.SetActive(true);
                    _mainMenuScreen.gameObject.SetActive(false);
                    break;
            }
        }
    }
}