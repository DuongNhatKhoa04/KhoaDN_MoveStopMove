namespace MoveStopMove.Core.Events
{
    public readonly struct LevelStart
    {
        public readonly int LevelIndex;

        public LevelStart(int levelIndex)
        {
            this.LevelIndex = levelIndex;
        }
    }

    public readonly struct LevelCompleted
    {
        public readonly int LevelIndex;
        public readonly int KillCount;

        public LevelCompleted(int levelIndex, int killCount)
        {
            this.LevelIndex = levelIndex;
            this.KillCount = killCount;
        }
    }

    public readonly struct LevelFailed
    {
        public readonly int LevelIndex;

        public LevelFailed(int levelIndex)
        {
            this.LevelIndex = levelIndex;
        }
    }
}