using System;
using System.Collections.Generic;
using System.Linq;
using UI.Core;
using UI.Core.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Popups.Core
{
    [Serializable]
    public class PopupsManager : BaseWindowsManager<BasePopup>, IPopupsManager
    {
        [SerializeField]
        private GameObject _popupFader;

        private Dictionary<PopupType, PopupModelData> _popupModelsParsed = new();
        private List<BasePopup> _currentPopups = new();

        public override void OnConfigLoaded(WindowsConfig windowConfig)
        {
            base.OnConfigLoaded(windowConfig);

            foreach (PopupModelData pmd in WindowConfig.PopupModels)
            {
                if (!_popupModelsParsed.TryAdd(pmd.PopupType, pmd))
                {
                    Debug.LogError("There is already setup up " + pmd.PopupType + " cant fill it twice!");
                }
            }
        }

        public void ShowPopup(PopupType popupType)
        {
            if (!_popupModelsParsed.ContainsKey(popupType))
            {
                Debug.LogError("Not filled page by type: " + popupType);
                return;
            }

            PopupModelData popupModelData = _popupModelsParsed[popupType];
            var basePopup =
                DiContainer.InstantiatePrefabForComponent<BasePopup>(popupModelData.Template, _container);

            if (basePopup == null)
            {
                Debug.LogError(
                    "There is no BasePopup attached to : " + basePopup.gameObject.name + " of type " + popupType);
                return;
            }

            basePopup.PopupData = popupModelData;

            AddPopup(basePopup);
        }

        public void HidePopup(PopupType popupType)
        {
            BasePopup currentPopup = _currentPopups.FirstOrDefault(r => r.PopupData.PopupType == popupType);
            if (currentPopup != null)
            {
                RemovePopup(currentPopup);
            }
            else
            {
                Debug.LogError("There is no popup by type: " + popupType + " active now!");
            }
        }

        private void AddPopup(BasePopup popup)
        {
            _currentPopups.Add(popup);

            if (popup.PopupData.UseTotalFader)
            {
                if (_currentPopups.Count >= 1)
                {
                    _popupFader.SetActive(true);
                }
            }

            popup.Show();
        }

        private void RemovePopup(BasePopup popup)
        {
            if (_currentPopups.Contains(popup))
            {
                _currentPopups.Remove(popup);
                if (_currentPopups.Count <= 0)
                {
                    _popupFader.SetActive(false);
                }
                else
                {
                    ShowNextPopup();
                }
            }

            Object.Destroy(popup.gameObject);
        }

        private void ShowNextPopup()
        {
            BasePopup newTopPopup = GetTopPopup();
            if (newTopPopup != null)
            {
                newTopPopup.Show();
            }
        }

        public BasePopup GetPopup(PopupType type) =>
            _currentPopups.FirstOrDefault(popup => type == popup.PopupData.PopupType);

        private BasePopup GetTopPopup() => _currentPopups[^1];

        public void HideAllPopups()
        {
            if (_currentPopups is { Count: > 0 })
            {
                foreach (BasePopup popup in _currentPopups)
                {
                    RemovePopup(popup);
                }
            }
        }
    }
}