using MoveStopMove.Core.Interfaces;
using UnityEngine;

namespace MoveStopMove.Core.Events
{
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

    public readonly struct CombatHitEvent
    {
        public readonly IDamageable Victim;
        public readonly float RangeUpdate;
        public readonly int Coin;

        public CombatHitEvent(IDamageable victim, float rangeUpdate, int coin)
        {
            Victim = victim;
            RangeUpdate = rangeUpdate;
            Coin = coin;
        }
    }
}