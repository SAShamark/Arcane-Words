using Game;
using Game.Data;
using Services.Currencies;
using UI.Core.Data;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ScriptableObjectInstaller : MonoInstaller
    {
        [SerializeField]
        private CurrencyCollection _currencyCollection;

        [SerializeField]
        private GamePlayConfig _gamePlayConfig;

        [SerializeField]
        private LevelsConfig _levelsConfig;

        public override void InstallBindings()
        {
            InstallBindingAsSingle(_currencyCollection);
            InstallBindingAsSingle(_gamePlayConfig);
            InstallBindingAsSingle(_levelsConfig);
        }

        private void InstallBindingAsSingle<T>(T scriptableObject) where T : ScriptableObject
        {
            Container.Bind<T>().FromInstance(scriptableObject).AsSingle().NonLazy();
        }
    }
}