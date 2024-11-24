using Services.Currencies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private CurrencyType _type;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;
        [SerializeField] private string _format = "{0}";

        private CurrencyService _currencyService;
        private IBank _bank;

        [Inject]
        private void Construct(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        private void Awake()
        {
            _bank = _currencyService.GetCurrencyByType(_type);
        }

        private void Start()
        {
            _bank.OnCurrencyChanged += SetCurrencyText;
            SetCurrencyText(_bank.Currency);
            _image.sprite = _currencyService.CurrencyCollection.CurrencySprites[_type];
        }

        private void OnDestroy()
        {
            _bank.OnCurrencyChanged -= SetCurrencyText;
        }

        private void SetCurrencyText(int value)
        {
            _text.text = string.Format(_format, value);
        }
    }
}