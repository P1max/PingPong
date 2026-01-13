using Bonuses;
using Paddles;
using UnityEngine;

namespace Ball
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

        private void HandlePaddlesCollision(global::Ball.Ball ball, Collision2D collisionObject)
        {
        }

        public void HandleCollision(global::Ball.Ball ball, Collision2D collisionObject)
        {
            if (collisionObject.gameObject.TryGetComponent<PlayerPaddle>(out _))
            {
                ball.SetIsLastPlayerPaddleTouch(true);
                ball.SetMoveSpeed(ball.MoveSpeed * _BALL_MOVE_SPEED_MULTIPLIER);
                Debug.Log($"Direction is {ball.Direction}");
            }
            else if (collisionObject.gameObject.TryGetComponent<ComputerPaddle>(out _))
            {
                ball.SetIsLastPlayerPaddleTouch(false);
                ball.SetMoveSpeed(ball.MoveSpeed * _BALL_MOVE_SPEED_MULTIPLIER);
                Debug.Log($"Direction is {ball.Direction}");
            }
            else if (collisionObject.gameObject.TryGetComponent<BonusBase>(out _))
            {
            }
            else
            {
                // Means Wall
                ball.SetDirection(new Vector2(ball.Direction.x, ball.Direction.y * -1));
            }
        }
    }
}