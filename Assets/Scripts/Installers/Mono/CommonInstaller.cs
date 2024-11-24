using Zenject;

namespace Installers.Mono
{
    public class CommonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            StorageInstaller.Install(Container);
            CurrencyInstaller.Install(Container);
        }
    }
}