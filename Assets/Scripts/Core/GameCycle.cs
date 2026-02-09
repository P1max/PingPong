using BallLogic;
using Cysharp.Threading.Tasks;
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
        [Inject] private BallContactsHandler _ballContactsHandler;
        [Inject] private ComputerPaddle _computerPaddle;
        [Inject] private BonusSpawner _bonusSpawner;
        [Inject] private ScoreHandler _scoreHandler;
        [Inject] private RestartPanel _restartPanel;
        [Inject] private PlayerPaddle _playerPaddle;
        [Inject] private GameTimer _gameTimer;
        [Inject] private BallsPool _ballsPool;

        private bool _isPlaying;
        
        private void TimerEnd() => EndGame(_scoreHandler.GetScoreResult());

        private void OnEnable()
        {
            _gameTimer.OnTimeEnd += TimerEnd;
            _scoreHandler.OnRoundEnd += EndGame;
            _ballContactsHandler.OnRoundEnd += RespawnBall;
        }

        private void OnDisable()
        {
            _gameTimer.OnTimeEnd -= TimerEnd;
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
            _gameTimer.StartTimer();

            await SpawnBall();

            _bonusSpawner.StartSpawning();
        }

        private void EndGame(WinType type) // Если 1 = Player Win, 0 = Draw, -1 = Computer Win
        {
            _isPlaying = false;
            
            _bonusSpawner.StopSpawning();
            _bonusSpawner.ReturnBonuses();

            _restartPanel.Show(type);

            _gameTimer.PauseTimer();
        }

        private async void RespawnBall()
        {
            if (!_isPlaying) return;
            
            _gameTimer.PauseTimer();
            
            await UniTask.Delay(1000); 
            
            await SpawnBall();
        }

        private async UniTask SpawnBall()
        {
            if (!_isPlaying) return;

            var newBall = _ballsPool.GetBall();

            newBall.gameObject.transform.position = new Vector3(0, 0, 0);

            await UniTask.Delay(1500);
            
            if (!newBall || !_isPlaying) return;

            _gameTimer.ContinueTimer();
            
            newBall.SetDirection(new Vector2(1, Random.Range(-1f, 1f)));
            newBall.SetMoveSpeed(12f);
        }

        #region enum

        public enum WinType
        {
            Player = 1,
            Draw = 0,
            Computer = -1
        }

        #endregion
    }
}