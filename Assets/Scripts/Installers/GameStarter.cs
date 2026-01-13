using System.Threading.Tasks;
using Spawners;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Installers
{
    public class GameStarter
    {
        private const float _DEFAULT_SPEED = 12f;

        private readonly BallsPool _ballsPool;
        private readonly BonusSpawner _bonusSpawner;

        public GameStarter(BallsPool ballsPool, BonusSpawner bonusSpawner)
        {
            _ballsPool = ballsPool;
            _bonusSpawner = bonusSpawner;
        }

        public async Task StartGameAsync()
        {
            await Task.Delay(500);

            var newBall = _ballsPool.GetBall();

            newBall.gameObject.transform.position = new Vector3(0, 0, 0);

            await Task.Delay(1500);

            if (!newBall) return;

            newBall.SetDirection(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            newBall.SetMoveSpeed(_DEFAULT_SPEED);
            
            _bonusSpawner.StartSpawning();
        }
    }
}