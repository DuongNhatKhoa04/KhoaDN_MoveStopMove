using UnityEngine;

namespace MoveStopMove.Extensions.Observer
{
    public readonly struct LevelStart
    {
        public readonly int levelIndex;

        public LevelStart(int levelIndex)
        {
            this.levelIndex = levelIndex;
        }
    }

    public readonly struct LevelCompleted
    {
        public readonly int levelIndex;
        public readonly int killCount;

        public LevelCompleted(int levelIndex, int killCount)
        {
            this.levelIndex = levelIndex;
            this.killCount = killCount;
        }
    }

    public readonly struct LevelFailed
    {
        public readonly int levelIndex;

        public LevelFailed(int levelIndex)
        {
            this.levelIndex = levelIndex;
        }
    }

    public readonly struct HitTarget
    {
        public readonly GameObject victim;
        public readonly float rangeUpdate;

        public HitTarget(GameObject victim, float rangeUpdate)
        {
            this.victim = victim;
            this.rangeUpdate = rangeUpdate;
        }
    }

    public readonly struct BuyItem
    {
        public readonly string itemId;
        public readonly float price;

        public BuyItem(string itemId, float price)
        {
            this.itemId = itemId;
            this.price = price;
        }
    }


}