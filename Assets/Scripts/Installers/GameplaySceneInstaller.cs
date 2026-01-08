using Scripts.Boot;
using Zenject;

namespace Scripts
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Container.Bind<BonusSpawner>().AsSingle().NonLazy();
            Container.Bind<GameStarter>().AsSingle();
            Container.Bind<BallsPool>().AsSingle();
            
            Container.Bind<GameStarterRunner>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}