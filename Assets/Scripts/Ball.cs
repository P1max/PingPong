#define test
using UnityEngine;

namespace Scripts
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector2 _direction = Vector2.zero;

        private Rigidbody2D _ballRigidbody;

        public float MoveSpeed => _moveSpeed;
        public Vector2 Direction => _direction;

        public Ball SetSpeed(float speed)
        {
            _moveSpeed = speed;
            _ballRigidbody.linearVelocity = _direction.normalized * _moveSpeed;

            return this;
        }

        public Ball SetDirection(Vector2 direction)
        {
            _direction = direction;
            _ballRigidbody.linearVelocity = _direction.normalized * _moveSpeed;

            return this;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<PlayerPaddleController>(out _) || other.gameObject.TryGetComponent<ComputerPaddleMovement>(out _))
            {
                HandlePaddleCollision(other);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                _direction.y = -_direction.y;
            }
        }

        private void HandlePaddleCollision(Collision2D collision)
        {
            var paddle = collision.gameObject;
            var contact = collision.GetContact(0);

            var isFrontHit = Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y);

            if (isFrontHit)
            {
                var paddleY = paddle.transform.position.y;
                var paddleHeight = paddle.GetComponent<Collider2D>().bounds.size.y;
                var ballY = transform.position.y;
                var yOffset = (ballY - paddleY) / (paddleHeight / 2f);
                
                yOffset = Mathf.Clamp(yOffset, -1f, 1f);

                var bounceStrength = 1f;
                
                _direction = new Vector2(-_direction.x, yOffset * bounceStrength).normalized;
            }
            else
            {
                _direction.y = -_direction.y;
            }
        }

        private void Awake()
        {
            _ballRigidbody = GetComponent<Rigidbody2D>();
        }
    }
}