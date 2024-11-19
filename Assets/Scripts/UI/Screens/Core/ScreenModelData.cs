using System;
using UnityEngine;

namespace UI.Screens.Core
{
    [Serializable]
    public class ScreenModelData
    {
        [SerializeField] private ScreenType _screenType;
        [SerializeField] private BaseScreen _template;
        [SerializeField] private bool _isAddToStack;
        
        public ScreenType ScreenType => _screenType;
        public BaseScreen Template => _template;
        public bool IsAddToStack => _isAddToStack;
    }
}