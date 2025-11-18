using MoveStopMove.Core.Combat;
using MoveStopMove.Gameplay.Projectiles;
using UnityEngine;

namespace MoveStopMove.Gameplay.Weapons
{
    public class ReturnableWeapon : WeaponBase
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
                AttackStrategy = new ReturnableAttackStrategy(this);
            }
        }

        public override void Attack(Vector3 targetPosition)
        {
            AttackStrategy.PerformAttack(targetPosition);
        }

        public ProjectileBase SpawnReturnableProjectile(Vector3 targetPosition)
        {
            var pooledObject = ProjectileObjectPool.Get();

            pooledObject.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            pooledObject.Initialize(attacker, targetPosition);
            return pooledObject;
        }

        #endregion
    }
}