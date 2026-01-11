using Scripts;
using UnityEngine;

namespace Paddles
{
    public class BallCollisionsHandler
    {
        private const float _BALL_MOVE_SPEED_MULTIPLIER = 1.1f;
        
        private readonly PlayerPaddle _playerPaddle;
        private ComputerPaddle _computerPaddle;

        public BallCollisionsHandler(PlayerPaddle playerPaddle, ComputerPaddle computerPaddle)
        {
            _playerPaddle = playerPaddle;
            _computerPaddle = computerPaddle;
        }
        
        public void HandleCollision(Ball ball, Collision2D collisionObject)
        {
            if (collisionObject.gameObject.TryGetComponent<PlayerPaddle>(out _))
            {
                _playerPaddle.IncreaseSize();
                ball.SetMoveSpeed(ball.MoveSpeed * _BALL_MOVE_SPEED_MULTIPLIER);
                
                return;
            }
        }
    }
}