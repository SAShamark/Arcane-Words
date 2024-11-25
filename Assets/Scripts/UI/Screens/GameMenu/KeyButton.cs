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

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private bool _isTriggerAnimator;

        private readonly int _isPressedHash = Animator.StringToHash("IsPressed");
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

        public void ResetIsPressed()
        {
            SetIsPressed(false);
        }

        private void ButtonClicked()
        {
            SetIsPressed(true);
            OnButtonClicked?.Invoke(this);
        }

        private void SetIsPressed(bool value)
        {
            if (_animator != null)
            {
                _keyText.gameObject.SetActive(!value);
                if (_isTriggerAnimator)
                {
                    _animator.SetTrigger(_isPressedHash);
                }
                else
                {
                    _animator.SetBool(_isPressedHash, value);
                }
            }
        }
    }
}