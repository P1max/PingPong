using System;
using Settings;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameTimer : ITickable
    {
        private readonly GameConfig _config;
        
        private float _timer;
        private bool _isTimerWorking;

        public event Action OnTimeEnd;

        public GameTimer(GameConfig config) => _config = config;
        
        public void PauseTimer()
        {
            _isTimerWorking = false;
        }

        public void ContinueTimer()
        {
            _isTimerWorking = true;
        }

        public void StartTimer()
        {
            _timer = 0;
            
            _isTimerWorking = true;
        }

        public void Tick()
        {
            if(!_isTimerWorking) return;
            
            _timer += Time.deltaTime;
            
            Debug.Log(_timer);

            if (_timer >= _config.Timer.GameDuration)
            {
                _isTimerWorking = false;
                
                OnTimeEnd?.Invoke();
            }
        }
    }
}