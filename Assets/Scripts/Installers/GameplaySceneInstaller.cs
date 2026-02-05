using BallLogic;
using Bonuses;
using Core;
using InputSystem;
using Paddles;
using Spawners;
using UI.RestartPanel;
using Zenject;

namespace Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BallsPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<BonusSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<BonusManager>().AsSingle().NonLazy();
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
            Container.Bind<RestartPanel>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}