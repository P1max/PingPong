using Zenject;

namespace Scripts
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BonusSpawner>().AsSingle();
            Container.Bind<BallSpawner>().AsSingle();
        }
    }
}