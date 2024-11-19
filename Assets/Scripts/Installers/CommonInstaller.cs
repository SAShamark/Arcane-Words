using Zenject;

namespace Installers
{
    public class CommonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            StorageInstaller.Install(Container);
            CurrencyInstaller.Install(Container);
            ClockInstaller.Install(Container);
        }
    }
}