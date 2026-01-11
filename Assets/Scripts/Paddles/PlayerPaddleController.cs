using InputSystem;
using UnityEngine;

namespace Scripts
{
    public class PlayerPaddleController
    {
        private readonly IInputReader _inputReader;
        private readonly PlayerPaddle _playerPaddle;

        private int _isInverted;

        public PlayerPaddleController(IInputReader inputReader, PlayerPaddle playerPaddle)
        {
            _inputReader = inputReader;
            _playerPaddle = playerPaddle;
            _isInverted = 1;
            
            _inputReader.OnUpAction += OnUp;
            _inputReader.OnDownAction += OnDown;
            _inputReader.OnReleaseAction += OnRelease;
            
            Debug.Log("PlayerPaddleController initialized");
        }

        private void OnUp()
        {
            Debug.Log($"Paddle Controller Up");
            
            _playerPaddle.Move(1 * _isInverted);
        }

        private void OnDown()
        {
            Debug.Log($"Paddle Controller Down");
            
            _playerPaddle.Move(-1 * _isInverted);
        }

        private void OnRelease()
        {
            Debug.Log($"Paddle Controller Release");
            
            _playerPaddle.Move(0);
        }
    }
}