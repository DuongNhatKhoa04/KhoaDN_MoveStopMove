using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Extensions.Observer;
using MoveStopMove.Managers;
using MoveStopMove.SO;
using UnityEngine;

namespace MoveStopMove.Weapon.Projectile
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

            var attackRangeBuff = PlayerSaveLoader.GetDecoratorData<WeaponData, float>(
                weaponName,
                PlayerSaveLoader.SO_WEAPON_PATH,
                data => data.rangeIncrease);

            EventManager.Instance.Notify(new HitTarget(Owner, attackRangeBuff, target));

            ReturnToPool();
        }

        #endregion
    }
}