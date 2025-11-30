using UnityEngine;

namespace MoveStopMove.Core.Events
{
    public readonly struct HitTarget
    {
        public readonly GameObject Victim;
        public readonly GameObject Target;
        public readonly int Coin;

        public HitTarget(GameObject victim, GameObject target, int coin)
        {
            this.Victim = victim;
            this.Target = target;
            this.Coin = coin;
        }
    }
}