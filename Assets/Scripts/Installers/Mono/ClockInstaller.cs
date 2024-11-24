using Services.Clock;
using UnityEngine;
using Zenject;

namespace Installers.Mono
{
    public class ClockInstaller : MonoInstaller
    {
        [SerializeField]
        private ClockService _clockService;
        public override void InstallBindings()
        {
            Container.Bind<IClockService>().To<ClockService>().FromInstance(_clockService).AsSingle();
        }
    }
}