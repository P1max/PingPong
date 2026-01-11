using InputSystem;
using Paddles;
using Scripts.Boot;
using Zenject;

namespace Scripts
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Container.Bind<BonusSpawner>().AsSingle().NonLazy();
            Container.Bind<GameStarter>().AsSingle().NonLazy();
            Container.Bind<BallsPool>().AsSingle().NonLazy();
            Container.Bind<PlayerPaddleController>().AsSingle().NonLazy();
            Container.Bind<IInputReader>().To<InputReader>().AsSingle().NonLazy();
            Container.Bind<BallCollisionsHandler>().AsSingle().NonLazy();
            
            Container.Bind<GameStarterRunner>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<PlayerPaddle>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<ComputerPaddle>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}