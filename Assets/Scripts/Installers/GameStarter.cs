using System.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Scripts
{
    public class GameStarter
    {
        private const float _DEFAULT_SPEED = 5f;
        
        private BallsPool _ballsPool;
        private BonusSpawner _bonusSpawner;
        
        public GameStarter(BallsPool ballsPool)
        {
            _ballsPool = ballsPool;
            //_bonusSpawner = bonusSpawner;
        }

        public async Task StartGameAsync()
        {
            await Task.Delay(1000);

            var newBall = _ballsPool.GetBall();

            newBall.gameObject.transform.position = new Vector3(0, 0, 0);

            await Task.Delay(1500);
            
            newBall.SetDirection(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).SetSpeed(_DEFAULT_SPEED);
        }
    }
}