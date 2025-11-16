using MoveStopMove.Extensions.Strategy;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;

namespace MoveStopMove.Weapon
{
    public class PiercingWeapon : WeaponBase
    {
        #region -- Fields --

        [Header("Spawn Settings")]
        [SerializeField] private Transform firePoint;

        private IAttackStrategy m_attackStrategy;

        #endregion

        protected override void Awake()
        {
            base.Awake();

            if (firePoint == null)
            {
                firePoint = transform;
            }

            m_attackStrategy = new PiercingAttackStrategy(this);
        }

        public override void Attack(Vector3 targetPosition)
        {
            if (m_attackStrategy == null)
            {
                m_attackStrategy = new PiercingAttackStrategy(this);
            }

            m_attackStrategy.PerformAttack(targetPosition);
        }

        public ProjectileBase SpawnPiercingProjectile(Vector3 targetPosition)
        {
            if (firePoint == null)
            {
                firePoint = transform;
            }

            var pooledObject = ProjectileObjectPool.Get();

            pooledObject.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            pooledObject.Initialize(attacker, targetPosition);
            return pooledObject;
        }
    }
}