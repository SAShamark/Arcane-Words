using UnityEngine;
using Zenject;

namespace UI.Core
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField]
        protected Canvas _canvas;

        protected IUIManager UIManager { get; private set; }

        [Inject]
        private void Construct(IUIManager uiManager)
        {
            UIManager = uiManager;
        }
    }
}