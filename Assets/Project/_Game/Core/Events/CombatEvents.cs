using UnityEngine;

namespace MoveStopMove.Core.Events
{
    public readonly struct HitTarget
    {
        public readonly GameObject Victim;
        public readonly GameObject Target;

        public HitTarget(GameObject victim, GameObject target)
        {
            this.Victim = victim;
            this.Target = target;
        }
    }
}