using MoveStopMove.Managers;
using UnityEngine;

namespace MoveStopMove.Weapon.Projectile
{
    public class NormalProjectile : ProjectileBase
    {
        protected override void OnHitTarget(GameObject target)
        {
            if (target != null && Owner != null)
                Debug.Log($"NormalProjectile: {target.name} bị trúng bởi {Owner.name}");
            base.OnHitTarget(target);

            EventManager.Notify(new CharacterKilled(Owner, target, 0.2f));
        }
    }
}