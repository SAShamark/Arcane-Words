using Services.Currencies;
using Services.Storage;
using Zenject;

namespace Installers
{
    public class CurrencyInstaller : Installer<CurrencyInstaller>
    {
        private IStorageService _storageService;
        private CurrencyCollection _currencyCollection;

        [Inject]
        private void Construct(IStorageService storageService, CurrencyCollection currencyCollection)
        {
            _storageService = storageService;
            _currencyCollection = currencyCollection;
        }

        public override void InstallBindings()
        {
            BindCurrencyService();
        }

        private void BindCurrencyService()
        {
            var currencyService = new CurrencyService(_storageService, _currencyCollection);
            currencyService.Init();
            Container.Bind<CurrencyService>().FromInstance(currencyService).AsSingle();
        }
    }
}