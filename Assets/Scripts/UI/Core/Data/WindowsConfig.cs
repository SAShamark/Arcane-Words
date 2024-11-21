using UI.Popups.Core;
using UI.Screens.Core;
using UnityEngine;

namespace UI.Core
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "ScriptableObjects/UI/UI config", order = 1)]
    public class WindowsConfig : ScriptableObject
    {
        [SerializeField] private ScreenModelData[] _screenModels;
        [SerializeField] private PopupModelData[] _popupModels;

        public ScreenModelData[] ScreenModels => _screenModels;
        public PopupModelData[] PopupModels => _popupModels;
    }
}