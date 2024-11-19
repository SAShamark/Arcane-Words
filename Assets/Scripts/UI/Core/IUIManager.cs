using UI.Popups.Core;
using UI.Screens.Core;

namespace UI.Core
{
    public interface IUIManager
    {
        IScreensManager ScreensManager { get; }
        IPopupsManager PopupsManager { get; }
    }
}