#define test
using Paddles;
using UnityEngine;

namespace Scripts
{
    public class  Ball : MonoBehaviour
    {
        private const float _MAX_MOVE_SPEED = 30f;
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector2 _direction = Vector2.zero;

        private BallCollisionsHandler _ballCollisionsHandler;
        private Rigidbody2D _ballRigidbody;

        public float MoveSpeed => _moveSpeed;
        public Vector2 Direction => _direction;

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

        public void SetDependencies(BallCollisionsHandler ballCollisionsHandler)
        {
            _ballCollisionsHandler = ballCollisionsHandler;
        }

        private void OnCollisionEnter2D(Collision2D other) => _ballCollisionsHandler.HandleCollision(this, other);

        private void Awake()
        {
            _ballRigidbody = GetComponent<Rigidbody2D>();
        }
    }
}