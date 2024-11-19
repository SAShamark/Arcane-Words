using UI.Popups.Core;
using UI.Screens.Core;
using UnityEngine;
using Zenject;

namespace UI.Core
{
    public class ProjectCanvas : MonoBehaviour, IUIManager
    {
        [SerializeField]
        private ScreensManager _screensManager;

        [SerializeField]
        private PopupsManager _popupsManager;

        [SerializeField]
        private WindowsConfig _windowsConfig;
        
        private DiContainer _diContainer;
        
        public IScreensManager ScreensManager => _screensManager;
        public IPopupsManager PopupsManager => _popupsManager;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
            _screensManager.Initialize(_diContainer);
            _popupsManager.Initialize(_diContainer);
            ConfigLoaded();
        }
        
        private void ConfigLoaded()
        {
            _screensManager.OnConfigLoaded(_windowsConfig);
            _popupsManager.OnConfigLoaded(_windowsConfig);
        }
    }
}