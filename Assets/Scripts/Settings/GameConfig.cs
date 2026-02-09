using Bonuses;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Tools/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public TimerConfig Timer;
        public ScoreConfig Score;
        public BallConfig Ball;
        public BonusSpawnConfig BonusSpawn;
        public BonusDurationsConfig BonusDurations;
    }

    [System.Serializable]
    public class TimerConfig
    {
        public float GameDuration = 60f;
    }

    [System.Serializable]
    public class ScoreConfig
    {
        public uint ScoreToWin = 5;
    }

    [System.Serializable]
    public class BallConfig
    {
        public float MaxSpeed = 30f;
        public float SpeedMultiplier = 1.05f;
    }

    [System.Serializable]
    public class BonusSpawnConfig
    {
        public float SpawnInterval = 12f;
    }

    [System.Serializable]
    public class BonusDurationEntry
    {
        public BonusType Type;
        public float Duration;
    }

    [System.Serializable]
    public class BonusDurationsConfig
    {
        public BonusDurationEntry[] Durations;
    }
}