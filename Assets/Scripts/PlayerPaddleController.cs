using System.Collections;
using UnityEngine;

public class PlayerPaddleController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;

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
        _verticalInput = _inputReader.GetVerticalInput(); 
        _paddleRigidbody.velocity = new Vector2(0, _verticalInput * moveSpeed);

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