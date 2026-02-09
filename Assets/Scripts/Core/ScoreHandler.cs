using System;
using Gates;
using Settings;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ScoreHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _computerScoreText;
        [SerializeField] private TextMeshProUGUI _playerScoreText;

        [Inject] private GameConfig _config;
        
        private uint _computerScore;
        private uint _playerScore;

        public event Action<GameCycle.WinType> OnRoundEnd;

        public void UpdateScore(Side goalSide)
        {
            if (goalSide == Side.Right)
            {
                _computerScore++;
                _computerScoreText.text = _computerScore.ToString();

                if (_computerScore >= _config.Score.ScoreToWin) OnRoundEnd?.Invoke(GameCycle.WinType.Computer);
            }
            else if (goalSide == Side.Left)
            {
                _playerScore++;
                _playerScoreText.text = _playerScore.ToString();

                if (_playerScore >= _config.Score.ScoreToWin) OnRoundEnd?.Invoke(GameCycle.WinType.Player);
            }
        }

        public GameCycle.WinType GetScoreResult()
        {
            if (_computerScore < _playerScore) return GameCycle.WinType.Player;
            if (_computerScore > _playerScore) return GameCycle.WinType.Computer;
            
            return GameCycle.WinType.Draw;
        }

        public void ResetScore()
        {
            _computerScore = 0;
            _playerScore = 0;
            _computerScoreText.text = _computerScore.ToString();
            _playerScoreText.text = _playerScore.ToString();
        }
    }
}