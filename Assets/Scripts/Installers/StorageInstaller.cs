using Services.Storage;
using Zenject;

namespace Installers
{
    public class StorageInstaller : Installer<StorageInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IStorageService>().To<StorageService>().AsSingle().NonLazy();
        }
    }
}