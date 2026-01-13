using System.Collections.Generic;
using Ball;
using UnityEngine;

namespace Spawners
{
    public class BallsPool
    {
        private readonly BallCollisionsHandler _ballCollisionsHandler;
        private readonly LinkedList<Ball.Ball> _activeBalls;
        private readonly LinkedList<Ball.Ball> _freeBalls;
        private readonly Ball.Ball _ballPrefab;

        public BallsPool(BallCollisionsHandler ballCollisionsHandler)
        {
            _ballCollisionsHandler = ballCollisionsHandler;
            _ballPrefab = Resources.Load<Ball.Ball>("Prefabs/Ball");
            
            _activeBalls = new LinkedList<Ball.Ball>();
            _freeBalls = new LinkedList<Ball.Ball>();
        }

        public Ball.Ball GetBall()
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

        public void ReturnBall(Ball.Ball ball)
        {
            ball.gameObject.SetActive(false);
            
            _activeBalls.Remove(ball);
            _freeBalls.AddLast(ball);
        }

        private Ball.Ball CreateBall()
        {
            var ball = Object.Instantiate(_ballPrefab);

            ball.SetDependencies(_ballCollisionsHandler, this);
            
            return ball;
        }
    }
}