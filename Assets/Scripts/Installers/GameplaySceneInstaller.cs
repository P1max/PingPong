using Ball;
using Boot;
using InputSystem;
using Paddles;
using Spawners;
using Zenject;

namespace Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Container.Bind<BonusSpawner>().AsSingle().NonLazy();
            Container.Bind<GameStarter>().AsSingle().NonLazy();
            Container.Bind<BallsPool>().AsSingle().NonLazy();
            Container.Bind<BonusSpawner>().AsSingle().NonLazy();
            Container.Bind<PlayerPaddleController>().AsSingle().NonLazy();
            Container.Bind<IInputReader>().To<InputReader>().AsSingle().NonLazy();
            Container.Bind<BallCollisionsHandler>().AsSingle().NonLazy();
            
            Container.Bind<GameCycle>()
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