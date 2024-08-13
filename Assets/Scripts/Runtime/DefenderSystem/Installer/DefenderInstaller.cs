using Runtime.DefenderSystem.Controller;
using Runtime.DefenderSystem.View;
using Zenject;

namespace Runtime.DefenderSystem.Installer
{
    public class DefenderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DefenderViewModel>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<DefenderDragController>().AsSingle();
            Container.BindInterfacesTo<DefenderVisualController>().AsSingle();
            Container.BindInterfacesTo<DefenderUpgradeController>().AsSingle();
            Container.BindInterfacesTo<DefenderAttackController>().AsSingle();
        }
    }
}