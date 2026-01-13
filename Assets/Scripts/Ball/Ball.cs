#define test
using System;
using Spawners;
using UnityEngine;

namespace Ball
{
    public class  Ball : MonoBehaviour, IDisposable
    {
        private const float _MAX_MOVE_SPEED = 30f;
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector2 _direction = Vector2.zero;

        private BallCollisionsHandler _ballCollisionsHandler;
        private BallsPool _ballsPool;
        private Rigidbody2D _ballRigidbody;
        private bool? _isLastPlayerPaddleTouch;

        public float MoveSpeed => _moveSpeed;
        public Vector2 Direction => _direction;
        public bool? IsLastPlayerPaddleTouch => _isLastPlayerPaddleTouch;

        public void SetDependencies(BallCollisionsHandler ballCollisionsHandler, BallsPool ballsPool)
        {
            _ballCollisionsHandler = ballCollisionsHandler;
            _ballsPool = ballsPool;
        }

        public void SetIsLastPlayerPaddleTouch(bool? isLastPlayerPaddleTouch) => _isLastPlayerPaddleTouch = isLastPlayerPaddleTouch;

        public void Dispose() => _ballsPool.ReturnBall(this);

        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = speed > _MAX_MOVE_SPEED ? _MAX_MOVE_SPEED : speed;
            _ballRigidbody.linearVelocity = _direction.normalized * _moveSpeed;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
            _ballRigidbody.linearVelocity = _direction.normalized * _moveSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other) => _ballCollisionsHandler.HandleCollision(this, other);

        private void Awake() => _ballRigidbody = GetComponent<Rigidbody2D>();

        private void OnEnable() => _isLastPlayerPaddleTouch = false;
    }
}