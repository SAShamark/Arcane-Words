namespace UI.Popups.Core
{
    public interface IPopupsManager
    {
        void ShowPopup(PopupType popupType);

        void HidePopup(PopupType popupType);
        BasePopup GetPopup(PopupType popupType);

        void HideAllPopups();
    }
}