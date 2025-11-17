using UnityEngine;

namespace MoveStopMove.Extensions.Observer
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

    public readonly struct HitTarget
    {
        public readonly GameObject Victim;
        public readonly GameObject Target;
        public readonly float RangeUpdate;

        public HitTarget(GameObject victim, float rangeUpdate, GameObject target)
        {
            this.Victim = victim;
            this.RangeUpdate = rangeUpdate;
            this.Target = target;
        }
    }

    public readonly struct BuyItem
    {
        public readonly string ItemId;
        public readonly float Price;

        public BuyItem(string itemId, float price)
        {
            this.ItemId = itemId;
            this.Price = price;
        }
    }


}