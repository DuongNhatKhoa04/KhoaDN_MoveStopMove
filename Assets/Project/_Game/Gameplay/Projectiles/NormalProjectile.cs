using MoveStopMove.Core.Events;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Gameplay.Projectiles
{
    public class NormalProjectile : ProjectileBase
    {
        #region -- Methods --

        protected override void OnHitTarget(GameObject target)
        {
            if (target == Owner)
                return;

            if ((hittableLayers.value & (1 << target.layer)) == 0)
            {
                return;
            }

            EventManager.Instance.Notify(new HitTarget(Owner, target, 1));

            ReturnToPool();
        }

        #endregion
    }
}