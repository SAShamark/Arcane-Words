using Audio;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class AudioInstaller : MonoInstaller
    {
        [SerializeField] private ProjectAudio _projectAudio;

        public override void InstallBindings()
        {
            var projectAudio = Container.InstantiatePrefabForComponent<ProjectAudio>(_projectAudio);
            projectAudio.gameObject.transform.SetParent(null);
            Container.BindInterfacesAndSelfTo<ProjectAudio>().FromInstance(projectAudio).AsSingle();
        }
    }
}