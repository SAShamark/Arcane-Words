namespace UI.Screens.Core
{
    public interface IScreensManager
    {
        void ShowScreen(ScreenType screenTypes);
        void ShowPreviousScreen();
        void RemoveAllScreens();
        BaseScreen GetScreen(ScreenType screenType);
    }
}