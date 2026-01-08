using System;
using UnityEngine;
using Zenject;

namespace Scripts.Boot
{
    public class GameStarterRunner : MonoBehaviour
    {
        [Inject] private GameStarter _gameStarter;

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
    }
}