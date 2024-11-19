using Services.Clock;
using Zenject;

namespace Installers
{
    public class ClockInstaller : Installer<ClockInstaller>
    {
        public override void InstallBindings()
        {
            var timerService = new ClockService();
            Container.BindInterfacesAndSelfTo<ClockService>().FromInstance(timerService).AsSingle();
        }
    }
}