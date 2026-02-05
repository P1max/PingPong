using UnityEngine;

namespace Paddles
{
    public class PlayerPaddle : BasePaddle
    {
        private Rigidbody2D _rigidbody;
        private float _moveSpeed;
        private int _direction;
        
        public int Direction => _direction;

        protected override void Awake()
        {
            base.Awake();
            
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Move(int direction) // 1 = up; -1 = down; 0 = release
        {
            _direction = direction;
            _rigidbody.linearVelocityY = direction * _MAX_MOVE_SPEED;
        }
    }
}