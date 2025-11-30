using MoveStopMove.Core.Events;
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

            EventManager.Instance.Notify(new HitTarget(Owner, target));

            ReturnToPool();
        }

        #endregion
    }
}