using System.Linq;
using BallLogic;
using Spawners;
using UnityEngine;
using Zenject;

namespace Paddles
{
    public class ComputerPaddle : BasePaddle
    {
        [SerializeField] private float _reactionTime = 0.1f; // Задержка реакции
        [SerializeField] private float _predictionAccuracy = 0.95f; // 0-1, где 1 = идеально

        [Inject] private BallsPool _ballsPool;

        private Rigidbody2D _rigidbody;
        private Vector3 _targetPosition;
        private float _reactionTimer;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody2D>();
            _targetPosition = transform.position;
            _reactionTimer = 0f;
        }

        private void Update()
        {
            // Уменьшаем счётчик реакции
            if (_reactionTimer > 0)
            {
                _reactionTimer -= Time.deltaTime;
                return;
            }

            // Получаем опасный мячик
            var targetBall = GetMostDangerousBall();

            if (targetBall && IsMovingTowardsPaddle(targetBall))
            {
                // Предсказываем точку столкновения
                _targetPosition = PredictBallCollisionPoint(targetBall);

                // Добавляем ошибку в зависимости от сложности
                _targetPosition.y += Random.Range(-0.5f, 0.5f) * (1f - _predictionAccuracy);

                // Запускаем реакцию
                _reactionTimer = _reactionTime;
            }
        }

        private void FixedUpdate()
        {
            MoveToTarget();
        }
        
        private Ball GetMostDangerousBall()
        {
            var activeBalls = _ballsPool.GetActiveBalls().Where(ball => ball.Direction.x < 0).ToArray();

            if (activeBalls.Length == 0)
                return null;

            Ball dangerousBall = null;
            var closestDistance = float.MaxValue;

            foreach (var ball in activeBalls)
            {
                var distance = Vector3.Distance(transform.position, ball.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    dangerousBall = ball;
                }
            }

            return dangerousBall;
        }

        private bool IsMovingTowardsPaddle(Ball ball)
        {
            // Если мячик справа от доски и движется влево = движется к доске
            if (transform.position.x < ball.transform.position.x && ball.Direction.x < 0)
                return true;

            return false;
        }

        private Vector3 PredictBallCollisionPoint(Ball ball)
        {
            var ballPos = ball.transform.position;
            var ballVelocity = ball.MoveSpeed * ball.Direction;

            // Если мячик не движется по X, отбиваем в центр доски
            if (Mathf.Abs(ballVelocity.x) < 0.01f)
                return transform.position;

            // Время, когда мячик достигнет линии доски (X координата)
            var timeToCollision = (transform.position.x - ballPos.x) / ballVelocity.x;

            // Если время отрицательное, мячик уже прошёл или движется в другую сторону
            if (timeToCollision < 0)
                return transform.position;

            // Предсказываем Y позицию мячика в момент столкновения
            var predictedY = ballPos.y + ballVelocity.y * timeToCollision;

            return new Vector3(transform.position.x, predictedY, transform.position.z);
        }

        /// <summary>
        /// Перемещает доску к целевой позиции
        /// </summary>
        private void MoveToTarget()
        {
            var distanceToTarget = _targetPosition.y - transform.position.y;

            // Если доска уже близко к цели, останавливаем
            if (Mathf.Abs(distanceToTarget) < 0.1f)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                return;
            }

            // Определяем направление движения
            var moveDirection = Mathf.Sign(distanceToTarget);

            // Движемся с максимальной скоростью
            _rigidbody.linearVelocity = new Vector2(0, moveDirection * _MAX_MOVE_SPEED);
        }
    }
}