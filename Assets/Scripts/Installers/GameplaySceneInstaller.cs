using BallLogic;
using Bonuses;
using Core;
using InputSystem;
using Paddles;
using Settings;
using Spawners;
using UI.RestartPanel;
using Zenject;

namespace Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public GameConfig _config;
        
        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromInstance(_config).AsSingle();
            Container.Bind<BallsPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<BonusSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<BonusManager>().AsSingle().NonLazy();
            Container.Bind<PlayerPaddleController>().AsSingle().NonLazy();
            Container.Bind<BallContactsHandler>().AsSingle().NonLazy();
            Container.Bind<IInputReader>().To<InputReader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameTimer>().AsSingle();

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