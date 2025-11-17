using MoveStopMove.Extensions.Strategy;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;

namespace MoveStopMove.Weapon
{
    public class PiercingWeapon : WeaponBase
    {
        #region -- Methods --

        protected override void Awake()
        {
            base.Awake();

            if (firePoint == null)
            {
                firePoint = transform;
            }

            if (AttackStrategy == null)
            {
                AttackStrategy = new PiercingAttackStrategy(this);
            }

        }

        public override void Attack(Vector3 targetPosition)
        {
            AttackStrategy.PerformAttack(targetPosition);
        }

        public ProjectileBase SpawnPiercingProjectile(Vector3 targetPosition)
        {
            var pooledObject = ProjectileObjectPool.Get();

            pooledObject.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            pooledObject.Initialize(attacker, targetPosition);
            return pooledObject;
        }

        #endregion
    }
}