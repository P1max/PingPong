using InputSystem;

namespace Paddles
{
    public class PlayerPaddleController
    {
        private readonly IInputReader _inputReader;
        private readonly PlayerPaddle _playerPaddle;

        private bool _isInverted;

        public int IsInverted
        {
            get
            {
                if (!_isInverted) return 1;
                
                return -1;
            }
        }

        public PlayerPaddleController(IInputReader inputReader, PlayerPaddle playerPaddle)
        {
            _inputReader = inputReader;
            _playerPaddle = playerPaddle;
            _isInverted = false;
            
            _inputReader.OnUpAction += OnUp;
            _inputReader.OnDownAction += OnDown;
            _inputReader.OnReleaseAction += OnRelease;
        }

        private void OnUp()
        {
            _playerPaddle.Move(1 * IsInverted);
        }

        private void OnDown()
        {
            _playerPaddle.Move(-1 * IsInverted);
        }

        private void OnRelease()
        {
            _playerPaddle.Move(0);
        }

        public void SetIsInverted(bool isInverted)
        {
            _isInverted = isInverted;
            
            _playerPaddle.Move(-1 * _playerPaddle.Direction);
        }
    }
}