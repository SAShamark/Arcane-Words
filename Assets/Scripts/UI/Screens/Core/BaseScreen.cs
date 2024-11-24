using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.Core
{
    public class BaseScreen : BaseWindow
    {
        [SerializeField] protected Button _backButton;

        public ScreenModelData ScreenData { get; set; }
        
        public virtual void Initialize()
        {
            if (_backButton != null)
            {
                _backButton.onClick.RemoveAllListeners();

                _backButton.onClick.AddListener(UIManager.ScreensManager.ShowPreviousScreen);
                _backButton.onClick.AddListener(ButtonClicked);
            }
        }

        public virtual void Show()
        {
            _canvas.enabled = true;
        }

        public virtual void Hide()
        {
            _canvas.enabled = false;
        }
    }
}