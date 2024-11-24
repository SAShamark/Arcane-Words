using UI.Core;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField]
        private ProjectCanvas _projectCanvas;

        public override void InstallBindings()
        {
            var projectCanvas = Container.InstantiatePrefabForComponent<ProjectCanvas>(_projectCanvas);
            projectCanvas.gameObject.transform.SetParent(null);
            Container.BindInterfacesAndSelfTo<ProjectCanvas>().FromInstance(projectCanvas).AsSingle();
        }
    }
}