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

        private char _sign;
        public event Action<char> OnButtonClicked;
        
        public void Init(char sign)
        {
            if (sign != '\0')
            {
                _sign = sign;
                _keyText.text = sign.ToString();
            }
            _button.onClick.AddListener(ButtonClicked);
        }
        private void ButtonClicked()
        {
            OnButtonClicked?.Invoke(_sign);
        }
    }
}