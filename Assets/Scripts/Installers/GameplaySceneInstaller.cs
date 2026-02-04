using BallLogic;
using Bonuses;
using Boot;
using Core;
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
            Container.Bind<BallsPool>().AsSingle();
            Container.Bind<BonusSpawner>().AsSingle();
            Container.Bind<BonusManager>().AsSingle().NonLazy();
            Container.Bind<PlayerPaddleController>().AsSingle().NonLazy();
            Container.Bind<BallContactsHandler>().AsSingle().NonLazy();
            Container.Bind<IInputReader>().To<InputReader>().AsSingle().NonLazy();

            Container.Bind<GameCycle>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<PlayerPaddle>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<ComputerPaddle>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<ScoreHandler>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}