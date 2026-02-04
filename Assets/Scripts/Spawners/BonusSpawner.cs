using Bonuses;
using UnityEngine;

namespace Spawners
{
    public class BonusSpawner
    {
        private const float _TIME_FOR_BONUS_SPAWN = 5;

        private readonly BonusBase[] _bonuses;
        private readonly int _count;

        private float _timer;
        private bool _isSpawningBonuses;

        public BonusSpawner()
        {
            _bonuses = Resources.LoadAll<BonusBase>("Prefabs");
            _count = _bonuses.Length;
            _isSpawningBonuses = false;
        }

        private void InstantiateBonus()
        {
            var bonusNumber = Random.Range(0, _count);
            var bonus = Object.Instantiate(_bonuses[bonusNumber]);

            bonus.transform.position = new Vector2(Random.Range(-10f, 10f), Random.Range(-6f, 6f));
        }

        public void ReturnBonus(BonusBase bonus)
        {
            Object.Destroy(bonus.gameObject);
        }

        public void StartSpawning()
        {
            _isSpawningBonuses = true;
        }

        public void StopSpawning()
        {
            _isSpawningBonuses = false;
            _timer = 0f;
        }
        
        public void Tick(float deltaTime)
        {
            if (!_isSpawningBonuses) return;
            
            _timer += deltaTime;
            
            if (_timer >= _TIME_FOR_BONUS_SPAWN)
            {
                _timer -= _TIME_FOR_BONUS_SPAWN;
                
                InstantiateBonus();
            }
        }
    }
}