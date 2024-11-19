using System;
using UnityEngine;

namespace UI.Popups.Core
{
    [Serializable]
    public class PopupModelData
    {
        [SerializeField] private PopupType _popupType;
        [SerializeField] private BasePopup _template;
        [SerializeField] private bool _useTotalFader;
        
        public PopupType PopupType => _popupType;
        public BasePopup Template => _template;
        public bool UseTotalFader => _useTotalFader;
    }
}