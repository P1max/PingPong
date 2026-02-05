using System;
using Gates;
using TMPro;
using UnityEngine;

namespace Core
{
    public class ScoreHandler : MonoBehaviour
    {
        private const uint _SCORE_TO_WIN = 3;

        [SerializeField] private TextMeshProUGUI _leftScoreText;
        [SerializeField] private TextMeshProUGUI _rightScoreText;

        private uint _leftScore;
        private uint _rightScore;

        public event Action<Side> OnRoundEnd;

        public void UpdateScore(Side side)
        {
            if (side == Side.Right)
            {
                _leftScore++;
                _leftScoreText.text = _leftScore.ToString();

                if (_leftScore >= _SCORE_TO_WIN) OnRoundEnd?.Invoke(Side.Left);
            }
            else if (side == Side.Left)
            {
                _rightScore++;
                _rightScoreText.text = _rightScore.ToString();

                if (_rightScore >= _SCORE_TO_WIN) OnRoundEnd?.Invoke(Side.Right);
            }
        }

        public void ResetScore()
        {
            _leftScore = 0;
            _rightScore = 0;
            _leftScoreText.text = _leftScore.ToString();
            _rightScoreText.text = _rightScore.ToString();
        }
    }
}