using System;
using Bonuses;
using Core;
using Paddles;
using Spawners;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BallLogic
{
    public class BallContactsHandler
    {
        private const float _BALL_MOVE_SPEED_MULTIPLIER = 1.1f;

        private readonly ScoreHandler _scoreHandler;
        private readonly BonusSpawner _bonusSpawner;
        private readonly BonusManager _bonusManager;
        private readonly BallsPool _ballsPool;

        public event Action OnGoal;

        public BallContactsHandler(ScoreHandler scoreHandler, BonusSpawner bonusSpawner, BonusManager bonusManager,
            BallsPool ballsPool)
        {
            _scoreHandler = scoreHandler;
            _bonusSpawner = bonusSpawner;
            _bonusManager = bonusManager;
            _ballsPool = ballsPool;

            _bonusManager.OnBallTwinRequested += SpawnTwin;
        }

        private void SpawnTwin(Ball original)
        {
            var twin = _ballsPool.GetBall();

            twin.transform.position = original.transform.position;

            var rnd = Random.Range(-0.2f, 0.2f);
            var dir = new Vector2(
                -original.Direction.x,
                original.Direction.y + rnd
            ).normalized;

            twin.SetDirection(dir);
            twin.SetMoveSpeed(original.MoveSpeed);
        }

        private void HandlePaddlesCollision(Ball ball, Collision2D collisionObject)
        {
            var paddleCenter = collisionObject.transform.position.y;
            var contactPoint = collisionObject.GetContact(0).point.y;

            var distanceBetweenPaddleCenterAndContact = contactPoint - paddleCenter;

            var paddleHalfHeight = collisionObject.transform.localScale.y / 2f;
            var newYDirection = (float)0.7 * distanceBetweenPaddleCenterAndContact / paddleHalfHeight;

            ball.SetDirection(new Vector2(-ball.Direction.x, newYDirection));
            ball.SetMoveSpeed(ball.MoveSpeed * _BALL_MOVE_SPEED_MULTIPLIER);
        }

        public void HandleCollision(Ball ball, Collision2D collisionObject)
        {
            if (collisionObject.gameObject.TryGetComponent<PlayerPaddle>(out _))
            {
                ball.SetIsLastPlayerPaddleTouch(true);

                HandlePaddlesCollision(ball, collisionObject);
            }
            else if (collisionObject.gameObject.TryGetComponent<ComputerPaddle>(out _))
            {
                ball.SetIsLastPlayerPaddleTouch(false);

                HandlePaddlesCollision(ball, collisionObject);
            }
            else
            {
                // Means Wall
                ball.SetDirection(new Vector2(ball.Direction.x, ball.Direction.y * -1));
            }
        }

        public void HandleGatesTrigger(Ball ball, Collider2D colliderObject)
        {
            if (colliderObject.gameObject.TryGetComponent<BonusBase>(out var bonus))
            {
                _bonusManager.ApplyBonus(bonus.BonusType, ball);

                _bonusSpawner.ReturnBonus(bonus);
            }
            else if (colliderObject.TryGetComponent<Gates.Gates>(out var gates))
            {
                var activeBalls = _ballsPool.GetActiveBalls();

                foreach (var activeBall in activeBalls)
                    activeBall.Blow();

                _scoreHandler.UpdateScore(gates.Side);
                OnGoal?.Invoke();
            }
            else
            {
                Debug.LogWarning($"Столкновение с непонятным триггером :O");
            }
        }
    }
}