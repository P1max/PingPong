using System.Collections.Generic;
using Bonuses;
using Settings;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class BonusSpawner : ITickable
    {
        private readonly List<BonusBase> _activeBonuses;
        private readonly BonusBase[] _bonusesTypes;
        private readonly GameConfig _config;
        private readonly int _count;

        private float _timer;
        private bool _isSpawningBonuses;

        public BonusSpawner(GameConfig config)
        {
            _config = config;
            
            _bonusesTypes = Resources.LoadAll<BonusBase>("Prefabs");
            _activeBonuses = new List<BonusBase>();
            _count = _bonusesTypes.Length;
            _isSpawningBonuses = false;
        }

        public void ReturnBonuses()
        {
            for (var i = _activeBonuses.Count - 1; i >= 0; i--) 
                ReturnBonus(_activeBonuses[i]);
        }

        public void ReturnBonus(BonusBase bonus)
        {
            _activeBonuses.Remove(bonus);
            
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

        public void Tick()
        {
            if (!_isSpawningBonuses) return;
            
            _timer += Time.deltaTime;
            
            if (_timer >= _config.BonusSpawn.SpawnInterval)
            {
                _timer -= _config.BonusSpawn.SpawnInterval;
                
                InstantiateBonus();
            }
        }

        private void InstantiateBonus()
        {
            var bonusNumber = Random.Range(0, _count);
            var bonus = Object.Instantiate(_bonusesTypes[bonusNumber]);
            
            _activeBonuses.Add(bonus);

            bonus.transform.position = new Vector2(Random.Range(-10f, 10f), Random.Range(-6f, 6f));
        }
    }
}