using System;
using System.Collections.Generic;
using Services.Storage;

namespace Services.Currencies
{
    public class CurrencyService : IDisposable
    {
        private readonly Dictionary<CurrencyType, IBank> _currencyBanks = new();
        private readonly IStorageService _storageService;
        public event Action<CurrencyType, int> OnNotEnough;

        public CurrencyCollection CurrencyCollection { get; private set; }

        public CurrencyService(IStorageService storageService, CurrencyCollection currencyCollection)
        {
            _storageService = storageService;
            CurrencyCollection = currencyCollection;
        }
        
        public void Init(Dictionary<string, int> defaultValues)
        {
            AddAllCurrencyBanks(defaultValues);
        }

        private void AddAllCurrencyBanks(Dictionary<string, int> defaultValues)
        {
            var currencies = _storageService.LoadData(StorageConstants.CURRENCIES, defaultValues);
            foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
            {
                int initialCurrency = currencies.GetValueOrDefault(currencyType.ToString(), 0);
                AddCurrencyBank(currencyType, initialCurrency);
            }
        }

        private void AddCurrencyBank(CurrencyType currencyType, int initialCurrency)
        {
            var bank = new CurrencyBank(currencyType, initialCurrency);
            bank.OnCurrencyChanged += currency => SaveCurrency(currencyType, currency);
            bank.OnNotEnough += NotEnoughCurrency;
            _currencyBanks[currencyType] = bank;
        }

        private void NotEnoughCurrency(CurrencyType type, int value) => OnNotEnough?.Invoke(type, value);


        private void SaveCurrency(CurrencyType currencyType, int value)
        {
            var currencies = _storageService.LoadData(StorageConstants.CURRENCIES, new Dictionary<string, int>());
            currencies[currencyType.ToString()] = value;
            _storageService.SaveData(StorageConstants.CURRENCIES, currencies);
        }

        public IBank GetCurrencyByType(CurrencyType currencyType)
        {
            if (_currencyBanks.TryGetValue(currencyType, out IBank bank))
            {
                return bank;
            }

            throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null);
        }

        public void Dispose()
        {
            foreach (var bank in _currencyBanks)
            {
                bank.Value.OnNotEnough -= NotEnoughCurrency;
            }
        }
    }
}