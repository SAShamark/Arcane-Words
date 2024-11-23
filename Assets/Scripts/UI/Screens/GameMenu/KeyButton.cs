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

        public void Init(char sign)
        {
            if (sign != '\0')
            {
                Sign = sign;
                _keyText.text = sign.ToString();
            }

            _button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            _button.interactable = false;
            OnButtonClicked?.Invoke(this);
        }
    }
}