using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class PlayerPaddleController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 7f;

        private InputReader _inputReader;
        private Coroutine _inversionCoroutine;

        private Rigidbody2D _paddleRigidbody;
        private float _verticalInput;

        private void Awake()
        {
            _inputReader = new InputReader();
            _paddleRigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // _verticalInput = _inputReader.GetVerticalInput();
            // _paddleRigidbody.linearVelocity = new Vector2(0, _verticalInput * _moveSpeed);
        }

        public void ApplyInversion(float duration)
        {
            if (_inversionCoroutine != null) StopCoroutine(_inversionCoroutine);
            _inversionCoroutine = StartCoroutine(InvertTemporarily(duration));
        }

        private IEnumerator InvertTemporarily(float duration)
        {
            _inputReader.IsInverted = true;
            yield return new WaitForSeconds(duration);
            _inputReader.IsInverted = false;
            _inversionCoroutine = null;
        }
    }
}