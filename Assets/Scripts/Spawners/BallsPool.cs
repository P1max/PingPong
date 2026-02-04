using System.Collections.Generic;
using System.Linq;
using BallLogic;
using UnityEngine;
using Zenject;

namespace Spawners
{
    public class BallsPool
    {
        private readonly DiContainer _container;
        private readonly LinkedList<Ball> _activeBalls;
        private readonly LinkedList<Ball> _freeBalls;
        private readonly Ball _ballPrefab;
        
        public Ball[] GetActiveBalls() => _activeBalls.ToArray();

        public BallsPool(DiContainer container)
        {
            _container = container;
            _ballPrefab = Resources.Load<Ball>("Prefabs/Ball");
            
            _activeBalls = new LinkedList<Ball>();
            _freeBalls = new LinkedList<Ball>();
        }

        public Ball GetBall()
        {
            if (_freeBalls.Count > 0)
            {
                var ball = _freeBalls.First.Value;
                
                ball.gameObject.SetActive(true);
                ball.Show();
                
                _freeBalls.RemoveFirst();
                _activeBalls.AddLast(ball);

                return ball;
            }
            
            var newBall = CreateBall();
            
            newBall.gameObject.SetActive(true);
            
            _activeBalls.AddLast(newBall);
            
            return newBall;
        }

        public void ReturnBall(Ball ball)
        {
            ball.gameObject.SetActive(false);
            
            _activeBalls.Remove(ball);
            _freeBalls.AddLast(ball);
        }

        private Ball CreateBall()
        {
            var ball = _container.InstantiatePrefabForComponent<Ball>(_ballPrefab);
            
            return ball;
        }
    }
}