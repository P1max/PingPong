using System;
using Installers;
using Spawners;
using UnityEngine;
using Zenject;

namespace Boot
{
    public class GameCycle : MonoBehaviour
    {
        [Inject] private GameStarter _gameStarter;
        [Inject] private BonusSpawner _bonusSpawner;

        private async void Start()   // тут async void ок
        {
            try
            {
                await _gameStarter.StartGameAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Update()
        {
            _bonusSpawner.Tick(Time.deltaTime);
        }
    }
}