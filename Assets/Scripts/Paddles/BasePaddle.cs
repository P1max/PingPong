using UnityEngine;

namespace Scripts
{
    public class BasePaddle : MonoBehaviour
    {
        protected const float _MAX_MOVE_SPEED = 10f;
        protected const float _SIZE_MULTIPLIER = 0.5f;
        
        private float _defaultSizeY;

        protected virtual void Awake() => _defaultSizeY = transform.localScale.y;

        public void IncreaseSize() => transform.localScale = new Vector2(transform.localScale.x, _defaultSizeY + _defaultSizeY * _SIZE_MULTIPLIER);
        
        public void DecreaseSize() => transform.localScale = new Vector2(transform.localScale.x, _defaultSizeY - _defaultSizeY * _SIZE_MULTIPLIER);
        
        public void RestoreSize() => transform.localScale = new Vector2(transform.localScale.x, _defaultSizeY);
    }
}