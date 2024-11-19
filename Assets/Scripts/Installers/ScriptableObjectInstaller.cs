using Services.Currencies;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ScriptableObjectInstaller : MonoInstaller
    {
        [SerializeField]
        private CurrencyCollection _currencyCollection;


        public override void InstallBindings()
        {
            InstallBindingAsSingle(_currencyCollection);
        }

        private void InstallBindingAsSingle<T>(T scriptableObject) where T : ScriptableObject
        {
            Container.Bind<T>().FromInstance(scriptableObject).AsSingle().NonLazy();
        }
    }
}