using BallLogic;
using Cysharp.Threading.Tasks;
using Gates;
using Paddles;
using Spawners;
using UI.RestartPanel;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Core
{
    public class GameCycle : MonoBehaviour
    {
        [Inject] private BallsPool _ballsPool;
        [Inject] private BonusSpawner _bonusSpawner;
        [Inject] private ScoreHandler _scoreHandler;
        [Inject] private BallContactsHandler _ballContactsHandler;
        [Inject] private RestartPanel _restartPanel;
        [Inject] private PlayerPaddle _playerPaddle;
        [Inject] private ComputerPaddle _computerPaddle;

        private bool _isPlaying;

        private void OnEnable()
        {
            _scoreHandler.OnRoundEnd += EndGame;
            _ballContactsHandler.OnRoundEnd += RespawnBall;
        }

        private void OnDisable()
        {
            _scoreHandler.OnRoundEnd -= EndGame;
            _ballContactsHandler.OnRoundEnd -= RespawnBall;
        }
        
        private void Start()
        {
            _restartPanel.Init(() =>
            {
                _scoreHandler.ResetScore();
                _playerPaddle.transform.position = new Vector3(_playerPaddle.transform.position.x, 0, _playerPaddle.transform.position.z);
                _computerPaddle.transform.position = new Vector3(_computerPaddle.transform.position.x, 0, _computerPaddle.transform.position.z);
                
                StartGame().Forget();
            });
            
            StartGame().Forget();
        }

        private async UniTask StartGame()
        {
            await UniTask.Delay(500);
            
            _isPlaying = true;

            await SpawnBall();

            _bonusSpawner.StartSpawning();
        }

        private void EndGame(Side winSide)
        {
            _scoreHandler.OnRoundEnd -= EndGame;

            _bonusSpawner.StopSpawning();
            _bonusSpawner.ReturnBonuses();

            var isWin = winSide == Side.Right;
            
            _restartPanel.Show(isWin);
            
            _isPlaying = false;
        }

        private async void RespawnBall()
        {
            if (!_isPlaying) return;
            
            await UniTask.Delay(1000); 
            
            if (!_isPlaying) return;
            
            await SpawnBall();
        }

        private async UniTask SpawnBall()
        {
            var newBall = _ballsPool.GetBall();

            newBall.gameObject.transform.position = new Vector3(0, 0, 0);

            await UniTask.Delay(1500);

            if (!newBall) return;

            newBall.SetDirection(new Vector2(1, Random.Range(-1f, 1f)));
            newBall.SetMoveSpeed(12f);
        }
    }
}