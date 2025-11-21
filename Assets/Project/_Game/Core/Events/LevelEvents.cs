namespace MoveStopMove.Core.Events
{
    public enum ELevelAction
    {
        Start,
        Win,
        Lose,
        Pause,
        Resume,
        Restart,
        Quit
    }

    public readonly struct LevelEvent
    {
        public readonly ELevelAction Action;
        public readonly int Level;
        public readonly int Coin;

        public LevelEvent(ELevelAction action, int level, int coin)
        {
            Action = action;
            Level = level;
            Coin = coin;
        }
    }
}