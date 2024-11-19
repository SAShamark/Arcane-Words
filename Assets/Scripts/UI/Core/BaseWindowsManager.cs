using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Core
{
    [Serializable]
    public abstract class BaseWindowsManager<T> where T : BaseWindow
    {
        [SerializeField] protected Transform _container;
        
        protected WindowsConfig WindowConfig;
        protected DiContainer DiContainer;
        
        public T ActiveWindow { get; protected set; }
        public virtual void Initialize(DiContainer diContainer)
        {
            DiContainer = diContainer;
        }

        public virtual void OnConfigLoaded(WindowsConfig windowConfig)
        {
            WindowConfig = windowConfig;
        }

        protected void RemoveScreen(T window)
        {
            Object.Destroy(window.gameObject);
        }
    }
}