using System;
using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace UI.Screens.Core
{
    [Serializable]
    public class ScreensManager : BaseWindowsManager<BaseScreen>, IScreensManager
    {
        private readonly Dictionary<ScreenType, ScreenModelData> _screenModelsParsed = new();
        private readonly Dictionary<ScreenType, BaseScreen> _screens = new();
        private readonly Stack<BaseScreen> _previousScreens = new();

        public BaseScreen GetScreen(ScreenType screenType)
        {
            BaseScreen baseScreen = null;
            foreach (KeyValuePair<ScreenType, BaseScreen> screen in _screens)
            {
                if (screen.Key == screenType)
                {
                    baseScreen = screen.Value;
                }
            }

            return baseScreen;
        }
        
        public override void OnConfigLoaded(WindowsConfig windowConfig)
        {
            base.OnConfigLoaded(windowConfig);

            foreach (ScreenModelData screenModelData in WindowConfig.ScreenModels)
            {
                if (!_screenModelsParsed.TryAdd(screenModelData.ScreenType, screenModelData))
                {
                    Debug.LogError("There is already set up " + screenModelData.ScreenType + " cant fill it twice!");
                }
            }
        }

        public void ShowScreen(ScreenType screenTypes)
        {
            if (ActiveWindow != null)
            {
                if (ActiveWindow.ScreenData.IsAddToStack)
                {
                    _previousScreens.Push(ActiveWindow);
                    ActiveWindow.Hide();
                }
                else
                {
                    RemoveScreen(ActiveWindow);
                    _screens.Remove(ActiveWindow.ScreenData.ScreenType);
                }
            }

            if (_screens.TryGetValue(screenTypes, out BaseScreen screen))
            {
                screen.Show();
                ActiveWindow = screen;
            }
            else
            {
                AddScreen(screenTypes);
            }
        }

        private void AddScreen(ScreenType screenType)
        {
            if (!_screenModelsParsed.ContainsKey(screenType))
            {
                Debug.LogError("Not filled screen by type: " + screenType);
                return;
            }

            ScreenModelData screenModelData = _screenModelsParsed[screenType];
            var baseScreen =
                DiContainer.InstantiatePrefabForComponent<BaseScreen>(screenModelData.Template, _container);
            if (baseScreen == null)
            {
                Debug.LogError(
                    "There is no BasePage attached to : " + baseScreen.gameObject.name + " of type " + screenType);
                return;
            }

            baseScreen.ScreenData = screenModelData;
            _screens.Add(screenType, baseScreen);
            ActiveWindow = baseScreen;
            baseScreen.Initialize();
            baseScreen.Show();
        }

        public void ShowPreviousScreen()
        {
            if (_previousScreens.Count > 0)
            {
                BaseScreen previousScreenType = _previousScreens.Pop();
                ShowScreen(previousScreenType.ScreenData.ScreenType);
            }
        }

        public void RemoveAllScreens()
        {
            if (ActiveWindow != null)
            {
                RemoveScreen(ActiveWindow);
            }

            for (int i = 0; i < _previousScreens.Count; i++)
            {
                RemoveScreen(_previousScreens.Pop());
            }
        }
    }
}