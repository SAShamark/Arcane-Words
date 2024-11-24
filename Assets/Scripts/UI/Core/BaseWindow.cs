using Audio;
using Audio.Data;
using UnityEngine;
using Zenject;

namespace UI.Core
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField]
        protected Canvas _canvas;

        protected IUIManager UIManager { get; private set; }
        protected IAudioManager AudioManager { get; private set; }

        [Inject]
        private void Construct(IUIManager uiManager, IAudioManager audioManager)
        {
            UIManager = uiManager;
            AudioManager = audioManager;
        }
        protected void ButtonClicked()
        {
            AudioManager.Play(AudioGroupType.UiSounds, "Button");
        }
    }
}