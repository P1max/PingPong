#define test
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
   public class BallMovement : MonoBehaviour
   {
      [SerializeField] private float moveSpeed = 5f;
      [SerializeField] private Vector3 direction = Vector3.zero;
      private Rigidbody2D _ballRigidbody;

      private void Awake()
      {
         _ballRigidbody = GetComponent<Rigidbody2D>();
      }

      private void Start()
      {
         Launch();
      }

      private void Update()
      {
         _ballRigidbody.linearVelocity = direction.normalized * moveSpeed;
      }

      private void Launch()
      {
#if test
         direction = new Vector3(1f, Random.Range(-0.5f, 0.5f));
#endif
#if !test
      direction = new Vector3(Random.value < 0.5f ? -1f : 1f, Random.Range(-0.5f, 0.5f));
#endif
         _ballRigidbody.linearVelocity = direction * moveSpeed;
      }

      private void OnCollisionEnter2D(Collision2D other)
      {

         if (other.gameObject.TryGetComponent<PlayerPaddleController>(out _) ||
             other.gameObject.TryGetComponent<ComputerPaddleMovement>(out _))
         {
            HandlePaddleCollision(other);
         }
         else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
         {
            direction.y = -direction.y;
         }
      }

      private void HandlePaddleCollision(Collision2D collision)
      {
         GameObject paddle = collision.gameObject;
         ContactPoint2D contact = collision.GetContact(0);

         bool isFrontHit = Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y);

         if (isFrontHit)
         {
            float paddleY = paddle.transform.position.y;
            float paddleHeight = paddle.GetComponent<Collider2D>().bounds.size.y;

            float ballY = transform.position.y;

            float yOffset = (ballY - paddleY) / (paddleHeight / 2f);
            yOffset = Mathf.Clamp(yOffset, -1f, 1f);

            float bounceStrength = 1f;
            direction = new Vector2(-direction.x, yOffset * bounceStrength).normalized;
         }
         else
         {
            direction.y = -direction.y;
         }
      }
   }
}