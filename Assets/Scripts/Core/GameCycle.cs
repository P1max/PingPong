using System.Threading.Tasks;
using BallLogic;
using Bonuses;
using Core;
using Cysharp.Threading.Tasks;
using Gates;
using Spawners;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Boot
{
    public class GameCycle : MonoBehaviour
    {
        [Inject] private BallsPool _ballsPool;
        [Inject] private BonusSpawner _bonusSpawner;
        [Inject] private BonusManager _bonusManager;
        [Inject] private ScoreHandler _scoreHandler;
        [Inject] private BallContactsHandler _ballContactsHandler;

        private bool _isPlaying;

        public void OnDestroy()
        {
            _ballContactsHandler.OnGoal -= RespawnBall;
        }
        
        private async void Start()
        {
            _scoreHandler.OnGameEnd -= EndGame;
            _scoreHandler.OnGameEnd += EndGame;
            _ballContactsHandler.OnGoal += RespawnBall;
            
            await StartGame();
        }

        private async Task StartGame()
        {
            await Task.Delay(500);
            
            _isPlaying = true;

            var newBall = _ballsPool.GetBall();

            newBall.gameObject.transform.position = new Vector3(0, 0, 0);

            await Task.Delay(1500);

            if (!newBall) return;

            newBall.SetDirection(new Vector2(1, Random.Range(-1f, 1f)));
            newBall.SetMoveSpeed(12f);

            _bonusSpawner.StartSpawning();
        }

        private async void RespawnBall()
        {
            if (!_isPlaying) return;
            
            Debug.Log("Respawning Ball");
            
            await UniTask.Delay(1000);   
            
            await SpawnBall();
            
            Debug.Log("Respawned Ball");
        }

        private async Task SpawnBall()
        {
            var newBall = _ballsPool.GetBall();

            newBall.gameObject.transform.position = new Vector3(0, 0, 0);

            await Task.Delay(1500);

            if (!newBall) return;

            newBall.SetDirection(new Vector2(1, Random.Range(-1f, 1f)));
            newBall.SetMoveSpeed(12f);
        }

        private void EndGame(Side winSide)
        {
            _scoreHandler.OnGameEnd -= EndGame;
            
            _bonusSpawner.StopSpawning();
            
            _isPlaying = false;
        }

        private void Update()
        {
            var delta = Time.deltaTime;

            _bonusSpawner.Tick(delta);
            _bonusManager.Tick(delta);
        }
    }
}