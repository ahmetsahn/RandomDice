using Runtime.EnemySystem.Controller;
using Runtime.EnemySystem.View;
using Zenject;

namespace Runtime.EnemySystem.EnemyInstaller
{
    public class EnemyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EnemyViewModel>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<EnemyVisualController>().AsSingle();
            Container.BindInterfacesTo<EnemyMovementController>().AsSingle();
            Container.BindInterfacesTo<EnemyHealthController>().AsSingle();
        }
    }
}