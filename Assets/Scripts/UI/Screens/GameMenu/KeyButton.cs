using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.GameMenu
{
    public class KeyButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _keyText;

        [SerializeField]
        private Button _button;

        public Button Button => _button;

        public char Sign { get; private set; }

        public event Action<KeyButton> OnButtonClicked;

        private void Start()
        {
            _button.onClick.AddListener(ButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ButtonClicked);
        }

        public void Init(char sign)
        {
            if (sign != '\0')
            {
                Sign = sign;
                _keyText.text = sign.ToString();
            }
        }

        private void ButtonClicked()
        {
            OnButtonClicked?.Invoke(this);
        }
    }
}