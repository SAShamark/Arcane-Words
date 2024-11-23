using System.Collections.Generic;
using Game;
using Game.Data;
using Services.Currencies;
using Services.Storage;
using Zenject;

namespace Installers
{
    public class CurrencyInstaller : Installer<CurrencyInstaller>
    {
        private IStorageService _storageService;
        private CurrencyCollection _currencyCollection;
        private GamePlayConfig _gamePlayConfig;

        [Inject]
        private void Construct(IStorageService storageService, CurrencyCollection currencyCollection,
            GamePlayConfig gamePlayConfig)
        {
            _storageService = storageService;
            _currencyCollection = currencyCollection;
            _gamePlayConfig = gamePlayConfig;
        }

        public override void InstallBindings()
        {
            BindCurrencyService();
        }

        private void BindCurrencyService()
        {
            var currencyService = new CurrencyService(_storageService, _currencyCollection);
            var defaultValues = new Dictionary<string, int>
            {
                { CurrencyType.Hint.ToString(), _gamePlayConfig.StartHintCount }
            };
            currencyService.Init(defaultValues);
            Container.Bind<CurrencyService>().FromInstance(currencyService).AsSingle();
        }
    }
}