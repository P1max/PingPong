using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class BallsPool
    {
        private readonly LinkedList<Ball> _activeBalls;
        private readonly LinkedList<Ball> _freeBalls;
        private readonly Ball _ballPrefab;

        public BallsPool()
        {
            _ballPrefab = Resources.Load<Ball>("Prefabs/Ball");
            
            _activeBalls = new LinkedList<Ball>();
            _freeBalls = new LinkedList<Ball>();
            
            Debug.Log("Класс создан, префаб найден");
        }

        public Ball GetBall()
        {
            if (_freeBalls.Count > 0)
            {
                var ball = _freeBalls.First.Value;
                
                ball.gameObject.SetActive(true);
                
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
            var ball = Object.Instantiate(_ballPrefab);

            return ball;
        }
    }
}