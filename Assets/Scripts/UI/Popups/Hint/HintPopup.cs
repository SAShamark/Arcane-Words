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
        private bool _isUnlocked;
        private string _hintDescription;

        public event Action<string> OnHintUsed;

        [Inject]
        private void Construct(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public void Initialize(string word, string hintDescription, bool isUnlocked, int cost)
        {
            _cost = cost;
            _word = word;
            _hintDescription = hintDescription;
            _isUnlocked = isUnlocked;

            _acceptButton.onClick.AddListener(AcceptHint);
            _acceptButton.interactable = true;
            Draw();
        }

        private void Draw()
        {
            _headerText.text = _isUnlocked ? _word : new string('+', _word.Length);
            _bulbGameObject.SetActive(!_isUnlocked);
            _acceptText.text = _isUnlocked ? _acceptString : _cost.ToString();
            _acceptText.color = _isUnlocked ? _acceptTextColorWhenUnlocked : _acceptTextColorWhenLocked;
            _mainText.text = _hintDescription;
        }

        private void AcceptHint()
        {
            IBank hintCurrency = _currencyService.GetCurrencyByType(CurrencyType.Hint);
            if (hintCurrency.Currency > _cost)
            {
                hintCurrency.SpendCurrency(_cost);
                _blurGameObject.SetActive(false);
                _acceptButton.interactable = false;
                OnHintUsed?.Invoke(_word);
            }
        }
    }
}