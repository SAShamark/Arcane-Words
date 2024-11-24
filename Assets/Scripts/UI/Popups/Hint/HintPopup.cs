using System;
using Services.Currencies;
using TMPro;
using UI.Popups.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Popups.Hint
{
    public class HintPopup : BasePopup
    {
        [SerializeField]
        private GameObject _blurGameObject;

        [SerializeField]
        private TMP_Text _headerText;

        [SerializeField]
        private TMP_Text _mainText;

        [SerializeField]
        private TMP_Text _acceptText;

        [SerializeField]
        private GameObject _bulbGameObject;

        [SerializeField]
        private Button _acceptButton;

        [SerializeField]
        private string _acceptString = "Ok";

        [SerializeField]
        private Color _acceptTextColorWhenLocked;

        [SerializeField]
        private Color _acceptTextColorWhenUnlocked;

        private CurrencyService _currencyService;
        private int _cost;
        private string _word;
        private string _hintDescription;
        private bool _isHintUnlocked;
        private bool _isWordUnlocked;

        public event Action<string> OnHintUsed;

        [Inject]
        private void Construct(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public void Initialize(string word, string hintDescription, bool isHintUnlocked, bool isWordUnlocked, int cost)
        {
            _cost = cost;
            _word = word;
            _hintDescription = hintDescription;
            _isHintUnlocked = isHintUnlocked;
            _isWordUnlocked = isWordUnlocked;

            _acceptButton.onClick.AddListener(Accept);
            _acceptButton.interactable = true;
            Draw();
        }

        public override void CloseTrigger()
        {
            base.CloseTrigger();
            _acceptButton.onClick.AddListener(Accept);
        }

        private void Draw()
        {
            _headerText.text = _isWordUnlocked ? _word : new string('+', _word.Length);
            _blurGameObject.SetActive(!_isHintUnlocked);
            _bulbGameObject.SetActive(!_isHintUnlocked);
            _acceptText.text = _isHintUnlocked ? _acceptString : _cost.ToString();
            _acceptText.color = _isHintUnlocked ? _acceptTextColorWhenUnlocked : _acceptTextColorWhenLocked;
            _mainText.text = _hintDescription;
        }

        private void Accept()
        {
            if (_isHintUnlocked)
            {
                CloseTrigger();
                return;
            }

            TryUnlockHint();
        }

        private bool TryUnlockHint()
        {
            IBank hintCurrency = _currencyService.GetCurrencyByType(CurrencyType.Hint);
            if (hintCurrency.Currency > _cost)
            {
                hintCurrency.SpendCurrency(_cost);
                _blurGameObject.SetActive(false);
                _acceptButton.interactable = false;
                OnHintUsed?.Invoke(_word);
                return true;
            }

            return false;
        }
    }
}