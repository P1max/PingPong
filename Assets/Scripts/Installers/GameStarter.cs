namespace Scripts
{
    public class GameStarter
    {
        private BallSpawner _ballSpawner;
        private BonusSpawner _bonusSpawner;
        
        public GameStarter(BallSpawner ballSpawner, BonusSpawner bonusSpawner)
        {
            _ballSpawner = ballSpawner;
            _bonusSpawner = bonusSpawner;
            
               
        }
    }
}