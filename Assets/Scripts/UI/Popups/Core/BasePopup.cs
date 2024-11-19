using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups.Core
{
    public class BasePopup : BaseWindow
    {
        [SerializeField]
        private Button _closeButton;

        public PopupModelData PopupData { get; set; }

        public virtual void Show()
        {
            gameObject.SetActive(true);


            if (_closeButton != null)
            {
                _closeButton.onClick.RemoveAllListeners();

                _closeButton.onClick.AddListener(CloseTrigger);
            }
        }

        public virtual void CloseTrigger()
        {
            UIManager.PopupsManager.HidePopup(PopupData.PopupType);
        }
    }
}