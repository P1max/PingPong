using Cysharp.Threading.Tasks;
using Spawners;
using UnityEngine;
using Zenject;

namespace BallLogic
{
    public class Ball : MonoBehaviour
    {
        private const float _MAX_MOVE_SPEED = 30f;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector2 _direction = Vector2.zero;

        private BallContactsHandler _ballContactsHandler;
        private BallsPool _ballsPool;
        private ParticleSystem _particles;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private bool? _isLastPlayerPaddleTouch;

        public float MoveSpeed => _moveSpeed;
        public Vector2 Direction => _direction;
        public bool? IsLastPlayerPaddleTouch => _isLastPlayerPaddleTouch;

        [Inject]
        public void Construct(
            BallContactsHandler ballContactsHandler,
            BallsPool ballsPool)
        {
            _ballContactsHandler = ballContactsHandler;
            _ballsPool = ballsPool;
        }

        public void SetIsLastPlayerPaddleTouch(bool? isLastPlayerPaddleTouch) =>
            _isLastPlayerPaddleTouch = isLastPlayerPaddleTouch;
        
        public void Show() => _spriteRenderer.enabled = true;

        public void Blow()
        {
            _particles.Play();
            Hide();

            BlowAsync().Forget();
        }

        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = speed > _MAX_MOVE_SPEED ? _MAX_MOVE_SPEED : speed;
            _rigidbody.linearVelocity = _direction.normalized * _moveSpeed;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
            _rigidbody.linearVelocity = _direction.normalized * _moveSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other) => _ballContactsHandler.HandleGatesTrigger(this, other);

        private void OnCollisionEnter2D(Collision2D other) => _ballContactsHandler.HandleCollision(this, other);

        private void OnEnable() => _isLastPlayerPaddleTouch = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _particles = GetComponentInChildren<ParticleSystem>();
        }

        private async UniTaskVoid BlowAsync()
        {
            var cancelled = await UniTask.WaitUntil(() => _particles.isStopped, 
                    cancellationToken: this.GetCancellationTokenOnDestroy())
                .SuppressCancellationThrow();

            if (cancelled || this == null) return;

            _ballsPool.ReturnBall(this);
        }

        private void Hide()
        {
            _spriteRenderer.enabled = false;
            
            SetMoveSpeed(0);
        }
    }
}