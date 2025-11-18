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
}